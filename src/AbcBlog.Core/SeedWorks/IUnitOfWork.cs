using AbcBlog.Core.Repositories;

namespace AbcBlog.Core.SeedWorks
{
    public interface IUnitOfWork
    {
        IPostRepository Posts { get; }

        IPostCategoryRepository PostCategories { get; }

        ISeriesRepository Series { get; }
        Task<int> CompleteAsync();
    }
}
