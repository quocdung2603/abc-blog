using AbcBlog.Core;
using Microsoft.EntityFrameworkCore;

namespace AbcBlog.Api
{
    public static class MigrationManager
    {
        public static WebApplication MigrateDatabase (this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<AbcBlogContext>())
                {
                    context.Database.Migrate();
                    new DataSeeder().SeedAsync(context).Wait();
                }
            }
            return app;
        }
    }
}
