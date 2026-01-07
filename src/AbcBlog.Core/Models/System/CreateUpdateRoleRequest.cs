using AbcBlog.Core.Domain.Identity;
using AutoMapper;

namespace AbcBlog.Core.Models.System
{
    public class CreateUpdateRoleRequest
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}
