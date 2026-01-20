using AbcBlog.Core.ConfigOptions;
using AbcBlog.Core.Domain.Content;
using AbcBlog.Core.Domain.Identity;
using AbcBlog.Core.Helpers;
using AbcBlog.Core.SeedWorks;
using AbcBlog.Core.SeedWorks.Constants;
using AbcBlog.WebApp.Extensions;
using AbcBlog.WebApp.Models;
using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace AbcBlog.WebApp.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SystemConfig _systemConfig;

        public ProfileController(IUnitOfWork unitOfWork, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IOptions<SystemConfig> systemConfig)
        {
            _unitOfWork = unitOfWork;
            _signInManager = signInManager;
            _userManager = userManager;
            _systemConfig = systemConfig.Value;
        }

        [Route("/profile")]
        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUser();
            return View(new ProfileViewModel()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            });
        }

        [HttpGet]
        [Route("/profile/edit")]
        public async Task<IActionResult> ChangeProfile()
        {
            var user = await GetCurrentUser();
            return View(new ChangeProfileViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName
            });
        }

        [HttpPost]
        [Route("/profile/edit")]
        public async Task<IActionResult> ChangeProfile([FromForm] ChangeProfileViewModel model)
        {
            var user = await GetCurrentUser();
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData[SystemConsts.FormSuccessMsg] = "Update profile successful.";
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Update profile failed");

            }
            return View(model);
        }

        [HttpGet]
        [Route("/profile/change-password")]
        public IActionResult ChangePassword()
        {
            return View();
        }


        [HttpPost]
        [Route("/profile/change-password")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userProfile = await GetCurrentUser();

            var isPasswordValid = await _userManager.CheckPasswordAsync(userProfile, model.OldPassword);
            if (!isPasswordValid)
            {
                ModelState.AddModelError(string.Empty, "Old password is not correct");
                return View(model);
            }

            var result = await _userManager.ChangePasswordAsync(userProfile, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(userProfile);
                TempData[SystemConsts.FormSuccessMsg] = "Change password successful";
                return Redirect(UrlConsts.Profile);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            await HttpContext.SignOutAsync();

            return Redirect(UrlConsts.Home);
        }

        private async Task<AppUser> GetCurrentUser()
        {
            var userId = User.GetUserId();
            return await _userManager.FindByIdAsync(userId.ToString());
        }

        [HttpGet]
        [Route("/profile/posts/create")]
        public async Task<IActionResult> CreatePost()
        {

            return View(await SetCreatePostModel());
        }

        [HttpPost]
        [Route("/profile/posts/create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost([FromForm] CreatePostViewModel model, IFormFile? thumbnail)
        {
            if (!ModelState.IsValid)
            {
                return View(await SetCreatePostModel());
            }
            var user = await GetCurrentUser();
            var category = await _unitOfWork.PostCategories.GetByIdAsync(model.CategoryId);
            var post = new Post()
            {
                Name = model.Title,
                CategoryName = category.Name,
                CategorySlug = category.Slug,
                Slug = TextHelper.ToUnsignedString(model.Title),
                CategoryId = model.CategoryId,
                Content = model.Content,
                SeoDescription = model.SeoDescription,
                Status = PostStatus.Draft,
                AuthorUserId = user.Id,
                AuthorName = user.GetFullName(),
                AuthorUserName = user.UserName,
                Description = model.Description
            };
            _unitOfWork.Posts.Add(post);
            if (thumbnail != null)
            {
                await UploadThumbnail(thumbnail, post);
            }

            var result = await _unitOfWork.CompleteAsync();
            if (result > 0)
            {
                TempData[SystemConsts.FormSuccessMsg] = "Post is created successful.";
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Create post failed");

            }
            return View(model);
        }

        private async Task UploadThumbnail(IFormFile thumbnail, Post post)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_systemConfig.BackendApiUrl);

                using var stream = thumbnail.OpenReadStream();
                byte[] data;
                using (var br = new BinaryReader(stream))
                {
                    data = br.ReadBytes((int)stream.Length);
                }

                var bytes = new ByteArrayContent(data);

                var multiContent = new MultipartFormDataContent
                {
                    { bytes, "file", thumbnail.FileName }
                };

                var uploadResult = await client.PostAsync("api/admin/media?type=posts", multiContent);
                if (uploadResult.StatusCode != HttpStatusCode.OK)
                {
                    ModelState.AddModelError("", await uploadResult.Content.ReadAsStringAsync());
                }
                else
                {
                    var path = await uploadResult.Content.ReadAsStringAsync();
                    var pathObj = JsonSerializer.Deserialize<UploadResponse>(path);
                    post.Thumbnail = pathObj?.Path;
                }

            }
        }

        private async Task<CreatePostViewModel> SetCreatePostModel()
        {
            var model = new CreatePostViewModel()
            {
                Title = "Untitled",
                Categories = new SelectList(await _unitOfWork.PostCategories.GetAllAsync(), "Id", "Name")
            };
            return model;
        }

        [HttpGet]
        [Route("/profile/posts/list")]
        public async Task<IActionResult> ListPosts(string? keywork, int pageIndex = 1, int pageSize = 10)
        {
            var user = await GetCurrentUser();
            var post = await _unitOfWork.Posts.GetPostsByUserPagingAsync(keywork, user.Id, pageIndex, pageSize);
            var totalPost = post.Results.Count();
            var totalDraftPosts = post.Results.Where(x => x.Status == PostStatus.Draft).Count();
            var totalWaitingApprovalPosts = post.Results.Where(x => x.Status == PostStatus.WaitingForApproval).Count();
            var totalPublishedPosts = post.Results.Where(x => x.Status == PostStatus.Published).Count();
            var totalUnpaidPosts = post.Results.Where(x => x.Status == PostStatus.Published && x.IsPaid == false).Count();
            var totalUnpaidAmount = post.Results.Where(x => x.Status == PostStatus.Published && x.IsPaid == false).Sum(x => x.RoyaltyAmount);
            var totalPaidAmount = post.Results.Where(x => x.Status == PostStatus.Published && x.IsPaid == true).Sum(x => x.RoyaltyAmount);

            return View(new ListPostByUserViewModel()
            {
                Posts = post,
                Keyword = keywork,
                TotalPosts = totalPost,
                TotalDraftPosts = totalDraftPosts,
                TotalWaitingApprovalPosts = totalWaitingApprovalPosts,
                TotalPublishedPosts = totalPublishedPosts,
                TotalPaidAmount = totalPaidAmount,
                TotalUnpaidAmount = totalUnpaidAmount,
                TotalUnpaidPosts = totalUnpaidPosts,
            });
        }

        [HttpGet]
        [Route("/profile/posts/send-approve/{slug}")]
        public async Task<IActionResult> SendToApprove([FromRoute] string slug)
        {

            var post = await _unitOfWork.Posts.GetPostBySlug(slug);

            return View(new SendToApprove()
            {
                Post = post
            });
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmSendToApprove([FromRoute] string slug)
        {
            var user = await GetCurrentUser();
            var post = await _unitOfWork.Posts.GetPostBySlug(slug);
            await _unitOfWork.Posts.SendToApprove(post.Id, user.Id);
            var result = await _unitOfWork.CompleteAsync();
            if (result > 0)
            {
                TempData[SystemConsts.FormSuccessMsg] = "Post is created successful.";
                return Redirect(UrlConsts.PostListByUser);
            }
            else
            {
                return View();
            }
            
        }
    }
}
