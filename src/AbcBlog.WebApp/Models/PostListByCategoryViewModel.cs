using AbcBlog.Core.Models;
using AbcBlog.Core.Models.Content;

namespace AbcBlog.WebApp.Models
{
    public class PostListByCategoryViewModel
    {
        public PostCategoryDto Category { get; set;  }
        public PageResult<PostInListDto> Posts { get; set; }
    }
}
