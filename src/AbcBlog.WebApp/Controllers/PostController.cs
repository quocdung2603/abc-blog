using AbcBlog.Core.SeedWorks;
using AbcBlog.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AbcBlog.WebApp.Controllers
{
    public class PostController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public PostController(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        [Route("posts")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("posts/{categorySlug}")]
        public async Task<IActionResult> ListByCategory([FromRoute] string categorySlug, [FromQuery] int page = 1)
        {
            var post = await _unitOfWork.Posts.GetPostsByCategoryPagingAsync(categorySlug, page, 2);
            var category = await _unitOfWork.PostCategories.GetBySlug(categorySlug);
            return View(new PostListByCategoryViewModel() {
                Category = category,
                Posts = post
                
            });
        }

        [Route("tag/{tagSlug}")]
        public async Task<IActionResult> ListByTag([FromRoute] string tagSlug, [FromQuery] int page = 1)
        {
            var post = await _unitOfWork.Posts.GetPostByTagPagingAsync(tagSlug, page, 2);
            var tag = await _unitOfWork.Tags.GetBySlug(tagSlug);
            return View(new PostListByTagViewModel()
            {
                Posts = post,
                Tag = tag
            });
        }

        [Route("post/{slug}")]
        public async Task<IActionResult> Details([FromRoute] string slug)
        {
            var post = await _unitOfWork.Posts.GetPostBySlug(slug);
            var category = await _unitOfWork.PostCategories.GetBySlug(post.CategorySlug);
            var tag = await _unitOfWork.Tags.GetTagObjectByPostId(post.Id);
            return View(new PostDetailViewModel()
            {
                Post = post,
                Category = category,
                Tags = tag  
            });
        }
    }
}
