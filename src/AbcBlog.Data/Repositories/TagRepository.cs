using AbcBlog.Core;
using AbcBlog.Core.Domain.Content;
using AbcBlog.Core.Models;
using AbcBlog.Core.Models.Content;
using AbcBlog.Core.Repositories;
using AbcBlog.Data.SeedWorks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AbcBlog.Data.Repositories
{
    public class TagRepository : RepositoryBase<Tag, Guid>, ITagRepository
    {
        private readonly IMapper _mapper;
        public TagRepository(AbcBlogContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task AddTagToPost(Guid postId, Guid TagId)
        {
            var tagInPost = await _context.PostTags.FirstOrDefaultAsync(x => x.PostId == postId && x.TagId == TagId);
            if (tagInPost == null)
            {
                var newTagInPost = new PostTag
                {
                    PostId = postId,
                    TagId = TagId
                };
                await _context.PostTags.AddAsync(newTagInPost);
            }
        }

        public async Task DeleteTagToPost(Guid postId, Guid TagId)
        {
            var tagInPost = await _context.PostTags.FirstOrDefaultAsync(x => x.PostId == postId && x.TagId == TagId);
            if (tagInPost != null)
            {
                _context.PostTags.Remove(tagInPost);
            }
        }

        public async Task<PageResult<TagDto>> GetAllTagPagingAsync(string? keyword, int pageIndex = 1, int pageSize = 10)
        {
            var query = _context.Tags.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword));
            }

            var totalRow = await query.CountAsync();

            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return new PageResult<TagDto>
            {
                Results = await _mapper.ProjectTo<TagDto>(query).ToListAsync(),
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize
            };
        }

        public async Task<TagDto> GetBySlug(string slug)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(x => x.Slug == slug);
            if (tag == null) return null;
            return _mapper.Map<TagDto?>(tag);
        }

        public async Task<List<string>> GetTagByPostId(Guid postId)
        {
            var query = from post in _context.Posts
                        join pt in _context.PostTags on post.Id equals pt.PostId
                        join t in _context.Tags on pt.TagId equals t.Id
                        where post.Id == postId
                        select t.Name;
            return await query.ToListAsync();
        }

        public async Task<List<TagDto>> GetTagObjectByPostId(Guid postId)
        {
            var query = from p in _context.Posts
                        join pt in _context.PostTags on p.Id equals pt.PostId
                        join t in _context.Tags on pt.TagId equals t.Id
                        where pt.PostId == postId
                        select t;

            var totalRow = await query.CountAsync();

            return await _mapper.ProjectTo<TagDto>(query).ToListAsync();
        }

        public async Task<bool> HasPost(string tagName)
        {
            return await _context.Posts.AnyAsync(p => p.Tags != null && p.Tags.Contains(tagName));
        }

        public async Task<bool> isAlreadyExistingTag(string tagName)
        {
            return await _context.Tags.AnyAsync(x => x.Name.ToLower().Contains(tagName.ToLower()));
        }

        public Task<bool> IsTagInPost(Guid tagId, Guid postId)
        {
            return _context.PostTags.AnyAsync(x => x.PostId == postId && x.TagId == tagId);
        }
    }
}
