using AbcBlog.Core.Domain.Content;
using AbcBlog.Core.Models;
using AbcBlog.Core.Models.Content;
using AbcBlog.Core.SeedWorks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AbcBlog.Api.Controllers.AdminApi
{
    [Route("api/admin/post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IUnitOfWork _uniOfWork;
        private IMapper _mapper;

        public PostController(IUnitOfWork uniOfWork, IMapper mapper)
        {
            _uniOfWork = uniOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreatPost([FromBody] CreateUpdatePostRequest request)
        {
            var post = _mapper.Map<CreateUpdatePostRequest, Post>(request);
            _uniOfWork.Posts.Add(post);

            var result = await _uniOfWork.CompleteAsync();
            return result > 0 ? Ok() : BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost([FromBody] CreateUpdatePostRequest request, Guid id)
        {
            var post = await _uniOfWork.Posts.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            _mapper.Map(request, post);
            var result = await _uniOfWork.CompleteAsync();
            return result > 0 ? Ok() : BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePosts([FromQuery] Guid[] ids)
        {
            foreach (var id in ids)
            {
                var post = await _uniOfWork.Posts.GetByIdAsync(id);
                if (post == null)
                {
                    return NotFound();
                }
                _uniOfWork.Posts.Remove(post);
            }

            var result = await _uniOfWork.CompleteAsync();
            return result > 0 ? Ok() : BadRequest();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<PostDto>> GetPostById(Guid id)
        {
            var post = await _uniOfWork.Posts.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        [HttpGet]
        [Route("paging")]
        public async Task<ActionResult<PageResult<PostInListDto>>> GetPostsPaging(string? keyword, Guid? categoryId, int pageIndex, int pageSize = 10)
        {
            var result = await _uniOfWork.Posts.GetPostsPagingAsync(keyword, categoryId, pageIndex, pageSize);
            return Ok(result);
        }

    }
}
