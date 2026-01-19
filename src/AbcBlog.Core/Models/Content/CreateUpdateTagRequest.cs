using AbcBlog.Core.Domain.Content;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbcBlog.Core.Models.Content
{
    public class CreateUpdateTagRequest
    {
        public required string Name { get; set; }
        public string Slug { get; set; }
        public class AutoMapperProfiles : Profile
        {
            public AutoMapperProfiles()
            {
                CreateMap<CreateUpdateTagRequest, Tag>();
            }
        }
    }
}
