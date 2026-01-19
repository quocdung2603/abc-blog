using AbcBlog.Core.Domain.Content;
using AbcBlog.Core.Models;
using AbcBlog.Core.Models.Content;
using AbcBlog.Core.SeedWorks;

namespace AbcBlog.Core.Repositories
{
    public interface ITagRepository : IRepository<Tag, Guid>
    {
        Task<TagDto> GetBySlug(string slug);
        Task<PageResult<TagDto>> GetAllTagPagingAsync(string? keyword, int pageIndex = 1, int pageSize = 10);
        Task<bool> HasPost(string tagName);
        Task<bool> isAlreadyExistingTag(string tagName);
        Task AddTagToPost(Guid postId, Guid TagId);
        Task DeleteTagToPost(Guid postId, Guid TagId);
        Task<bool> IsTagInPost(Guid tagId, Guid postId);
        Task<List<string>> GetTagByPostId(Guid postId);
        Task<List<TagDto>> GetTagObjectByPostId(Guid postId);
    }
}
