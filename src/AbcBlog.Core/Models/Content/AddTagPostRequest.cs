using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbcBlog.Core.Models.Content
{
    public class AddTagPostRequest
    {
        public Guid PostId { get; set; }
        public Guid TagId { get; set; }
    }
}
