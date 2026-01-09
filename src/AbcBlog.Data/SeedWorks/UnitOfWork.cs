using AbcBlog.Core;
using AbcBlog.Core.Domain.Identity;
using AbcBlog.Core.Repositories;
using AbcBlog.Core.SeedWorks;
using AbcBlog.Data.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace AbcBlog.Data.SeedWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AbcBlogContext _context;

        public UnitOfWork(AbcBlogContext context, IMapper mapper, UserManager<AppUser> userManager)
        {
            _context = context;
            Posts = new PostRepository(context, mapper, userManager);
            PostCategories = new PostCategoryRepository(context, mapper);
            Series = new SeriesRepository(context, mapper);
        }

        public IPostRepository Posts { get; private set; }
        public IPostCategoryRepository PostCategories { get; private set; }
        public ISeriesRepository Series { get; private set; }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
