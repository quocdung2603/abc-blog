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
    [Route("api/admin/tags")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TagController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Permissions.Tags.Create)]
        public async Task<IActionResult> CreateTag([FromBody] CreateUpdateTagRequest request)
        {
            if (await _unitOfWork.Tags.isAlreadyExistingTag(request.Name) == true)
            {
                return BadRequest("Đã tồn tại Tag này rồi! Không tạo mới nữa");
            }
            var tag = _mapper.Map<CreateUpdateTagRequest, Tag>(request);
            _unitOfWork.Tags.Add(tag);

            var result = await _unitOfWork.CompleteAsync();

            return result > 0 ? Ok() : BadRequest();
        }

        [HttpPut("{id}")]
        [Authorize(Permissions.Tags.Edit)]
        public async Task<IActionResult> UpdateTag([FromBody] CreateUpdateTagRequest request, Guid id)
        {
            var tag = await _unitOfWork.Tags.GetByIdAsync(id);
            if (tag == null)
            {
                return NotFound();
            }
            _mapper.Map(request, tag);
            await _unitOfWork.CompleteAsync();

            return Ok();
        }

        [HttpDelete]
        [Authorize(Permissions.Tags.Delete)]
        public async Task<IActionResult> DeleteTags([FromBody] Guid[] ids)
        {
            foreach (var id in ids)
            {
                var tag = await _unitOfWork.Tags.GetByIdAsync(id);
                if (tag == null)
                {
                    return NotFound();
                }
                if (await _unitOfWork.Tags.HasPost(tag.Name))
                {
                    return BadRequest("Danh mục hiện đang có bài viết, không thể xóa");
                }
                _unitOfWork.Tags.Remove(tag);
            }

            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Ok() : BadRequest();
        }

        [HttpGet]
        [Authorize(Permissions.Tags.View)]
        public async Task<ActionResult<List<TagDto>>> GetAllTags()
        {
            var query = await _unitOfWork.Tags.GetAllAsync();
            var model = _mapper.Map<List<TagDto>>(query);
            return Ok(model);
        }

        [HttpGet("{id}")]
        [Authorize(Permissions.Tags.View)]
        public async Task<ActionResult<TagDto>> GetTagById(Guid id)
        {
            var tag = await _unitOfWork.Tags.GetByIdAsync(id);
            if (tag == null)
            {
                return NotFound();
            }
            var result = _mapper.Map<TagDto>(tag);
            return Ok(result);
        }

        [HttpGet]
        [Route("paging")]
        [Authorize(Permissions.Tags.View)]
        public async Task<ActionResult<PageResult<TagDto>>> GetAllTagsPaging(string? keywork, int pageIndex, int pageSize = 10)
        {
            var result = await _unitOfWork.Tags.GetAllTagPagingAsync(keywork, pageIndex, pageSize);
            return Ok(result);
        }

        [HttpGet]
        [Route("slug")]
        [Authorize(Permissions.Tags.View)]
        public async Task<ActionResult<TagDto>> GetTagBySlug(string slug)
        {
            var tag = await _unitOfWork.Tags.GetBySlug(slug);
            if (tag == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<TagDto>(tag);
            return Ok(result);

        }

        [HttpPut]
        [Route("post-tags")]
        [Authorize(Permissions.Tags.Edit)]
        public async Task<IActionResult> AddTagToPost([FromBody] AddTagPostRequest request)
        {
            var isTagInPost = await _unitOfWork.Tags.IsTagInPost(request.TagId, request.PostId);
            if (isTagInPost)
            {
                return BadRequest($"Tag đã được gán cho bài viết này rồi.");
            }

            await _unitOfWork.Tags.AddTagToPost(request.PostId, request.TagId);
            var result = await _unitOfWork.CompleteAsync();

            return result > 0 ? Ok() : BadRequest();
        }

        [HttpDelete]
        [Route("post-tags")]
        [Authorize(Permissions.Tags.Delete)]
        public async Task<IActionResult> DeleteTagToPost([FromBody] AddTagPostRequest request)
        {
            var isTagInPost = await _unitOfWork.Tags.IsTagInPost(request.TagId, request.PostId);
            if (!isTagInPost)
            {
                return NotFound();
            }

            await _unitOfWork.Tags.DeleteTagToPost(request.PostId, request.TagId);
            var result = await _unitOfWork.CompleteAsync();

            return result > 0 ? Ok() : BadRequest();
        }

        [HttpGet("tags/{postId}")]
        public async Task<ActionResult<List<string>>> GetAllPostTags(Guid postId)
        {
            var result = await _unitOfWork.Tags.GetTagByPostId(postId);
            return Ok(result);
        }

    }
}
