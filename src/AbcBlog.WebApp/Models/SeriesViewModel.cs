using AbcBlog.Core.Models;
using AbcBlog.Core.Models.Content;

namespace AbcBlog.WebApp.Models
{
    public class SeriesViewModel
    {
        public PageResult<SeriesInListDto> Series { get; set; }
    }
}
