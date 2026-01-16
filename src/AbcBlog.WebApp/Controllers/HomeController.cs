using AbcBlog.Core.Models.Content;
using AbcBlog.Core.SeedWorks;
using AbcBlog.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AbcBlog.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new HomeViewModel()
            {
                LastestPosts = await _unitOfWork.Posts.GetLastestPost(10)
            };
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
