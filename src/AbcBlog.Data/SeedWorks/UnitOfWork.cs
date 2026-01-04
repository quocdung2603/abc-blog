using AbcBlog.Core;
using AbcBlog.Core.Repositories;
using AbcBlog.Core.SeedWorks;
using AbcBlog.Data.Repositories;
using AutoMapper;

namespace AbcBlog.Data.SeedWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AbcBlogContext _context;

        public UnitOfWork(AbcBlogContext context, IMapper mapper)
        {
            _context = context;
            Posts = new PostRepository(context, mapper);
        }

        public IPostRepository Posts { get; private set; }
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
