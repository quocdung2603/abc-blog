using AbcBlog.Core.Domain.Identity;
using AbcBlog.Core.SeedWorks;

namespace AbcBlog.Core.Repositories
{
    public interface IUserRepository : IRepository<AppUser, Guid>
    {
        Task RemoveUserFromRoles(Guid userId, string[] roles);
    }
}
