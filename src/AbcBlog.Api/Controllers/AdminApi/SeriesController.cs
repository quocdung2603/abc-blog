using AbcBlog.Core.Domain.Content;
using AbcBlog.Core.Models;
using AbcBlog.Core.Models.Content;
using AbcBlog.Core.SeedWorks;
using AbcBlog.Core.SeedWorks.Constants;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AbcBlog.Api.Controllers.AdminApi
{
    [Route("api/admin/series")]
    [ApiController]
    public class SeriesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SeriesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Permissions.Series.Create)]
        public async Task<IActionResult> CreateSeries([FromBody] CreateUpdateSeriesRequest request)
        {
            var series = _mapper.Map<CreateUpdateSeriesRequest, Series>(request);
            _unitOfWork.Series.Add(series);

            var result = await _unitOfWork.CompleteAsync();

            return result > 0 ? Ok() : BadRequest();
        }

        [HttpPut("{id}")]
        [Authorize(Permissions.Series.Edit)]
        public async Task<IActionResult> UpdateSeries([FromBody] CreateUpdateSeriesRequest request, Guid id)
        {
            var series = await _unitOfWork.Series.GetByIdAsync(id);
            if (series == null)
            {
                return NotFound();
            }

            _mapper.Map(request, series);

            await _unitOfWork.CompleteAsync();

            return Ok();
        }

        [HttpDelete]
        [Authorize(Permissions.Series.Delete)]
        public async Task<IActionResult> DeleteSeries([FromBody] Guid[] ids)
        {
            foreach (var id in ids)
            {
                var series = await _unitOfWork.Series.GetByIdAsync(id);
                if (series == null)
                {
                    return NotFound();
                }

                if (await _unitOfWork.Series.HasPost(id))
                {
                    return BadRequest("Loạt bài đang chứa bài viết, không thể xóa");
                }

                _unitOfWork.Series.Remove(series);
            }

            return Ok();
        }

        [HttpGet("{id}")]
        [Authorize(Permissions.Series.View)]
        public async Task<ActionResult<SeriesDto>> GetSeriesById(Guid id)
        {
            var series = await _unitOfWork.Series.GetByIdAsync(id);
            if (series == null)
            {
                return NotFound();
            }

            return Ok(series);
        }

        [HttpGet]
        [Route("paging")]
        [Authorize(Permissions.Series.View)]
        public async Task<ActionResult<PageResult<SeriesInListDto>>> GetAllSeriesPaging(string? keyword, int pageIndex = 1, int pageSize = 10)
        {
            var result = await _unitOfWork.Series.GetSeriesPagingAsync(keyword, pageIndex, pageSize);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Permissions.Series.View)]
        public async Task<ActionResult<List<SeriesInListDto>>> GetAllSeries ()
        {
            var result = await _unitOfWork.Series.GetAllAsync();
            var series = _mapper.Map<List<SeriesInListDto>>(result);

            return Ok(series);
        }

        [HttpPut]
        [Route("post-series")]
        [Authorize(Permissions.Series.Edit)]
        public async Task<IActionResult> AddPostToSeries([FromBody] AddPostSeriesRequest request)
        {
            var isPostInSeries = await _unitOfWork.Series.IsPostInSeries(request.SeriesId, request.PostId);
            if(isPostInSeries)
            {
                return BadRequest($"Bài viết này đã nằm trong loạt bài.");
            }

            await _unitOfWork.Series.AddPostToSeries(request.SeriesId, request.PostId, request.SortOrder);
            var result = await _unitOfWork.CompleteAsync();

            return result > 0 ? Ok() : BadRequest();
        }

        [HttpDelete]
        [Route("post-series")]
        [Authorize(Permissions.Series.Delete)]
        public async Task<IActionResult> DeletePostToSeries([FromBody] AddPostSeriesRequest request)
        {
            var isPostInSeries = await _unitOfWork.Series.IsPostInSeries(request.SeriesId, request.PostId);
            if (!isPostInSeries)
            {
                return NotFound();
            }

            await _unitOfWork.Series.RemovePostToSeries(request.SeriesId, request.PostId);
            var result = await _unitOfWork.CompleteAsync();

            return result > 0 ? Ok() : BadRequest();
        }

        [HttpGet]
        [Route("post-series/{seriesId}")]
        [Authorize(Permissions.Series.View)]
        public async Task<ActionResult<List<PostInListDto>>> GetPostsInSeries(Guid seriesId)
        {
            var posts = await _unitOfWork.Series.GetAllPostsInSeries(seriesId);
            return Ok(posts);
        }
    }
}
