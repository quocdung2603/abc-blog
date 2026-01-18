using AbcBlog.Core;
using AbcBlog.Core.Domain.Content;
using AbcBlog.Core.Models;
using AbcBlog.Core.Models.Content;
using AbcBlog.Core.Repositories;
using AbcBlog.Data.SeedWorks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AbcBlog.Data.Repositories
{
    internal class PostCategoryRepository : RepositoryBase<PostCategory, Guid>, IPostCategoryRepository
    {
        private readonly IMapper _mapper;

        public PostCategoryRepository(AbcBlogContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<PostCategoryDto> GetBySlug(string Slug)
        {
            var category = await _context.PostCategories.FirstOrDefaultAsync(x => x.Slug == Slug);
            if (category == null)
            {
                throw new Exception($"Cannot find {Slug}");
            }
            return _mapper.Map<PostCategoryDto>(category);
        }

        public async Task<PageResult<PostCategoryDto>> GetPostCategoryPagingAsync(string? keyword, int pageIndex = 1, int pageSize = 10)
        {
            var query = _context.PostCategories.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword));
            }

            var totalRow = await query.CountAsync();

            query = query.OrderByDescending(x => x.DateCreated).Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return new PageResult<PostCategoryDto>
            {
                Results = await _mapper.ProjectTo<PostCategoryDto>(query).ToListAsync(),
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize

            };
        }

        public async Task<bool> HasPost(Guid categoryId)
        {
            return await _context.Posts.AnyAsync(x=> x.CategoryId == categoryId);
        }
    }
}
