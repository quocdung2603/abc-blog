using AbcBlog.Api.Extensions;
using AbcBlog.Core.Domain.Content;
using AbcBlog.Core.Domain.Identity;
using AbcBlog.Core.Models;
using AbcBlog.Core.Models.Content;
using AbcBlog.Core.SeedWorks;
using AbcBlog.Core.SeedWorks.Constants;
using AbcBlog.Data.SeedWorks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static AbcBlog.Core.SeedWorks.Constants.Permissions;

namespace AbcBlog.Api.Controllers.AdminApi
{
    [Route("api/admin/post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private IMapper _mapper;

        public PostController(IUnitOfWork uniOfWork, IMapper mapper, UserManager<AppUser> userManager) 
        {
            _unitOfWork = uniOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpPost]
        [Authorize(Permissions.Posts.Create)]
        public async Task<IActionResult> CreatPost([FromBody] CreateUpdatePostRequest request)
        {
            if (await _unitOfWork.Posts.IsSlugAlreadyExisted(request.Slug))
            {
                return BadRequest("Đã tồn tại slug này.");
            }
            var post = _mapper.Map<CreateUpdatePostRequest, Post>(request);
            var category = await _unitOfWork.PostCategories.GetByIdAsync(post.CategoryId);
            post.CategoryName = category.Name;
            post.CategorySlug = category.Slug;

            var userId = User.GetUserId();
            var user = await _userManager.FindByIdAsync(userId.ToString());
            post.AuthorUserId = userId;
            post.AuthorUserName = user.UserName;
            post.AuthorName = user.GetFullName();

            _unitOfWork.Posts.Add(post);

            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Ok() : BadRequest();
        }

        [HttpPut("{id}")]
        [Authorize(Permissions.Posts.Edit)]
        public async Task<IActionResult> UpdatePost([FromBody] CreateUpdatePostRequest request, Guid id)
        {
            var slugExisted = await _unitOfWork.Posts.IsSlugAlreadyExisted(request.Slug, id);
            if (slugExisted)
            {
                return BadRequest("Đã tồn tại slug này.");
            }
            var post = await _unitOfWork.Posts.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            if (post.CategoryId != request.CategoryId)
            {
                var category = await _unitOfWork.PostCategories.GetByIdAsync(request.CategoryId);
                post.CategoryName = category.Name;
                post.CategorySlug = category.Slug;
            }
            _mapper.Map(request, post);
            await _unitOfWork.CompleteAsync();

            return Ok();
        }

        [HttpDelete]
        [Authorize(Permissions.Posts.Delete)]
        public async Task<IActionResult> DeletePosts([FromQuery] Guid[] ids)
        {
            foreach (var id in ids)
            {
                var post = await _unitOfWork.Posts.GetByIdAsync(id);
                if (post == null)
                {
                    return NotFound();
                }
                _unitOfWork.Posts.Remove(post);
            }

            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Ok() : BadRequest();
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Permissions.Posts.View)]
        public async Task<ActionResult<PostDto>> GetPostById(Guid id)
        {
            var post = await _unitOfWork.Posts.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        [HttpGet]
        [Route("paging")]
        [Authorize(Permissions.Posts.View)]
        public async Task<ActionResult<PageResult<PostInListDto>>> GetPostsPaging(string? keyword, Guid? categoryId, int pageIndex, int pageSize = 10)
        {
            var userId = User.GetUserId();
            var result = await _unitOfWork.Posts.GetPostsPagingAsync(keyword, userId, categoryId, pageIndex, pageSize);
            return Ok(result);
        }

        [HttpGet]
        [Route("series-belong/{postId}")]
        [Authorize(Permissions.Posts.View)]
        public async Task<ActionResult<List<SeriesInListDto>>> GetSeriesBelong(Guid postId)
        {
            var result = await _unitOfWork.Posts.GetAllSeries(postId);
            return Ok(result);
        }



        [HttpGet("approve/{id}")]
        [Authorize(Permissions.Posts.Approve)]
        public async Task<IActionResult> ApprovePost(Guid id)
        {
            await _unitOfWork.Posts.Approve(id, User.GetUserId());
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        [HttpGet("approval-submit/{id}")]
        [Authorize(Permissions.Posts.Edit)]
        public async Task<IActionResult> SendToApprove(Guid id)
        {
            await _unitOfWork.Posts.SendToApprove(id, User.GetUserId());
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        [HttpPost("return-back/{id}")]
        [Authorize(Permissions.Posts.Approve)]
        public async Task<IActionResult> ReturnBack(Guid id, [FromBody] ReturnBackRequest model)
        {
            await _unitOfWork.Posts.ReturnBack(id, User.GetUserId(), model.Reason);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        [HttpGet("return-reason/{id}")]
        [Authorize(Permissions.Posts.Approve)]
        public async Task<ActionResult<string>> GetReason(Guid id)
        {
            var note = await _unitOfWork.Posts.GetReturnReason(id);
            return Ok(note);
        }

        [HttpGet("activity-logs/{id}")]
        [Authorize(Permissions.Posts.Approve)]
        public async Task<ActionResult<List<PostActivityLogDto>>> GetActivityLogs(Guid id)
        {
            var logs = await _unitOfWork.Posts.GetActivityLogs(id);
            return Ok(logs);
        }

    }
}
