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
        public override string ProfileName => "DomainMappingToDtoProfile";
        public DomainMappingToDtoProfile()
        {
            CreateMap<PostCategory, PostCategoryViewModel>();
            CreateMap<Post, PostViewModel>();
            CreateMap<Comment, CommentViewModel>();

            CreateMap<PostCategory, SimpleSelectItem>()
                .ForMember(x => x.Id, opt => opt.MapFrom(dt => dt.Id))
                .ForMember(x => x.Name, opt => opt.MapFrom(dt => dt.CategoryName));
            //
            CreateMap<PostCategoryViewModel, PostCategory>();
            CreateMap<PostViewModel, Post>();
            CreateMap<CommentViewModel, Comment>();
            CreateMap<UserViewModel, User>();
        }

    }
}
