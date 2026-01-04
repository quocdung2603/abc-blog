using AbcBlog.Core.Domain.Content;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbcBlog.Core.Models.Content
{
    public class CreateUpdatePostRequest
    {
        public Guid CategoryId { get; set; }

        public string? Content { get; set; }


        public string? Source { get; set; }

        public string? Tags { get; set; }

        public string? SeoDescription { get; set; }
        public required string Name { get; set; }
        public required string Slug { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
        public string? Thumbnail { get; set; }

        public class AutoMapperProfiles : Profile
        {
            public AutoMapperProfiles()
            {
                CreateMap<CreateUpdatePostRequest, Post>();
            }
        }
    }
}
