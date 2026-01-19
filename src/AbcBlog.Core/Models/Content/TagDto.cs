using AbcBlog.Core.Domain.Content;
using AutoMapper;

namespace AbcBlog.Core.Models.Content
{
    public class TagDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string Slug { get; set; }

        public class AutoMapperProfiles : Profile
        {
            public AutoMapperProfiles()
            {
                CreateMap<Tag, TagDto>();
            }
        }
    }
}
