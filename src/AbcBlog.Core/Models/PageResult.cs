using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbcBlog.Core.Models
{
    public class PageResult<T> : PageResultPage where T : class
    {
        public List<T> Results { get; set; }
        public PageResult()
        {
            Results = new List<T>();
        }
    }
}
