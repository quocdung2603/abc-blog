using AbcBlog.Core;
using AbcBlog.Core.SeedWorks;

namespace AbcBlog.Data.SeedWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AbcBlogContext _context;

        public UnitOfWork(AbcBlogContext context)
        {
            _context = context;
        }

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
