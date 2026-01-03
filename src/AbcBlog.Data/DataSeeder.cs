using AbcBlog.Core.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace AbcBlog.Core
{
    public class DataSeeder
    {
        public async Task SeedAsync(AbcBlogContext context)
        {
            var passwordHasher = new PasswordHasher<AppUser>();

            var rootAdminRoleId = Guid.NewGuid();
            if (!context.Roles.Any())
            {
                await context.Roles.AddAsync(new AppRole()
                {
                    Id = rootAdminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    DisplayName = "Quản trị viên"
                });

                await context.SaveChangesAsync();
            }

            if (!context.Users.Any())
            {
                var userId = Guid.NewGuid();
                var user = new AppUser()
                {
                    Id = userId,
                    FirstName = "Dung",
                    LastName = "Nguyen",
                    Email = "admin@abc.com.vn",
                    NormalizedEmail = "ADMIN@ABC.COM.VN",
                    UserName = "Admin",
                    NormalizedUserName = "ADMIN",
                    IsActive = true,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    LockoutEnabled = false,
                    DateCreated = DateTime.Now,
                };

                user.PasswordHash = passwordHasher.HashPassword(user, "Admin@123");
                await context.Users.AddAsync(user);

                await context.UserRoles.AddAsync(new IdentityUserRole<Guid>()
                {
                    RoleId = rootAdminRoleId,
                    UserId = userId
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
