using AbcBlog.Core.Domain.Content;
using AbcBlog.Core.Models;
using AbcBlog.Core.Models.Content;
using AbcBlog.Core.SeedWorks;
using AbcBlog.Core.SeedWorks.Constants;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AbcBlog.Api.Controllers.AdminApi
{
    [Route("api/admin/post-category")]
    [ApiController]
    public class PostCategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PostCategoryController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Permissions.PostCategories.Create)]
        public async Task<IActionResult> CreatePostCategory([FromBody] CreateUpdatePostCategoryRequest request)
        {
            var postCategory = _mapper.Map<CreateUpdatePostCategoryRequest, PostCategory>(request);
            _unitOfWork.PostCategories.Add(postCategory);

            var result = await _unitOfWork.CompleteAsync();

            return result > 0 ? Ok() : BadRequest();
        }

        [HttpPut("{id}")]
        [Authorize(Permissions.PostCategories.Edit)]
        public async Task<IActionResult> UpdatePostCategory([FromBody] CreateUpdatePostCategoryRequest request, Guid id)
        {
            var postCategory = await _unitOfWork.PostCategories.GetByIdAsync(id);
            if (postCategory == null)
            {
                return NotFound();
            }

            _mapper.Map(request, postCategory);

            await _unitOfWork.CompleteAsync();

            return Ok();
        }

        [HttpDelete]
        [Authorize(Permissions.PostCategories.Delete)]
        public async Task<IActionResult> DeletePostCategories([FromBody] Guid[] ids)
        {
            foreach (var id in ids)
            {
                var postCategory = await _unitOfWork.PostCategories.GetByIdAsync(id);
                if (postCategory == null)
                {
                    return NotFound();
                }
                _unitOfWork.PostCategories.Remove(postCategory);
            }

            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Ok() : BadRequest();
        }

        [HttpGet("{id}")]
        [Authorize(Permissions.PostCategories.View)]
        public async Task<ActionResult<PostCategoryDto>> GetPostCategoryById(Guid id)
        {
            var postCategory = await _unitOfWork.PostCategories.GetByIdAsync(id);
            if (postCategory == null)
            {
                return NotFound();
            }
            var result = _mapper.Map<PostCategoryDto>(postCategory);
            return Ok(result);
        }

        [HttpGet]
        [Route("paging")]
        [Authorize(Permissions.PostCategories.View)]
        public async Task<ActionResult<PageResult<PostCategoryDto>>> GetAllPostCategoryPaging(string? keyword, int pageIndex, int pageSize = 10)
        {
            var result = await _unitOfWork.PostCategories.GetPostCategoryPagingAsync(keyword, pageIndex, pageSize);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Permissions.PostCategories.View)]
        public async Task<ActionResult<List<PostCategoryDto>>> GetPostCategories()
        {
            var query = await _unitOfWork.PostCategories.GetAllAsync();
            var model = _mapper.Map<PostCategoryDto>(query);
            return Ok(model);
        }
    }
}
