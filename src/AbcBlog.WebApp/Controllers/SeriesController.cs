using AbcBlog.Core.SeedWorks;
using AbcBlog.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AbcBlog.WebApp.Controllers
{
    public class SeriesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public SeriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Route("/series")]
        public async Task<IActionResult> Index([FromQuery] int page =1)
        {
            var series = await _unitOfWork.Series.GetSeriesPagingAsync(string.Empty, page);
            return View(series);
        }

        [Route("/series/{slug}")]
        public async Task<IActionResult> Details([FromRoute] string slug)
        {
            var series = await _unitOfWork.Series.GetSeriesBySlug(slug);
            var posts = await _unitOfWork.Series.GetAllPostsInSeries(slug);
            return View(new SeriesDetailViewModel()
            {
                Series = series,
                Posts = posts
            });
        }
    }
}
