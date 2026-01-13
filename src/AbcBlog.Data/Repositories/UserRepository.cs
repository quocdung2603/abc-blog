using AbcBlog.Core;
using AbcBlog.Core.Domain.Identity;
using AbcBlog.Core.Repositories;
using AbcBlog.Data.SeedWorks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbcBlog.Data.Repositories
{
    internal class UserRepository : RepositoryBase<AppUser, Guid>, IUserRepository
    {
        public UserRepository(AbcBlogContext context) : base(context)
        {
        }

        public async Task RemoveUserFromRoles(Guid userId, string[] roles)
        {
            if (roles == null || roles.Length == 0)
                return;
            foreach (var roleName in roles)
            {
                var role = await _context.Roles.FirstOrDefaultAsync(x => x.Name == roleName);
                if (role == null)
                {
                    return;
                }
                var userRole = await _context.UserRoles.FirstOrDefaultAsync(x => x.RoleId == role.Id && x.UserId == userId);
                if (userRole == null)
                {
                    return;
                }
                _context.UserRoles.Remove(userRole);
            }
        }
    }
}
