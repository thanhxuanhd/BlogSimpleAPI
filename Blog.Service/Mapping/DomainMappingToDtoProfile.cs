using AutoMapper;
using Blog.Core.Model;
using Blog.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Service.Mapping
{
    public class DomainMappingToDtoProfile : Profile
    {
        public override string ProfileName => "DomainToDtoMappingProfile";
        public DomainMappingToDtoProfile()
        {
            CreateMap<PostCategory, PostCategoryViewModel>();
            CreateMap<PostCategory, SimpleSelectItem>()
                .ForMember(x => x.Id, opt => opt.MapFrom(dt => dt.Id))
                .ForMember(x => x.Name, opt => opt.MapFrom(dt => dt.CategoryName));
            CreateMap<Post, PostViewModel>();
            CreateMap<Comment, CommentViewModel>();
        }

    }
}
