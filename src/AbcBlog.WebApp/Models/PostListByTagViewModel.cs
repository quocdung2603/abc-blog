using AbcBlog.Core.Models;
using AbcBlog.Core.Models.Content;

namespace AbcBlog.WebApp.Models
{
    public class PostListByTagViewModel
    {
        public TagDto Tag { get; set; }
        public PageResult<PostInListDto> Posts { get; set; }
    }
}
