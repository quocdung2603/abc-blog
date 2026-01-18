using AbcBlog.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace AbcBlog.WebApp.Components
{
    public class PagerViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync (PageResultPage result)
        {
            return Task.FromResult((IViewComponentResult)View("Default", result));
        }
    }
}
