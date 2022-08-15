using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestNewLine.Core.Dtos;
using TestNewLine.Core.ViewModels;
using TestNewLine.Data.Models;

namespace TestNewLine.AutoMapper
{

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            
            //user
            CreateMap<User, UserViewModel>()
                .ForMember(x => x.UserType, x => x.MapFrom(x => x.UserType.ToString()));
            CreateMap<CreateUserDto, User>()
                .ForMember(x => x.ImageUrl, x => x.Ignore());
            CreateMap<UpdateUserDto, User>()
                .ForMember(x => x.ImageUrl, x => x.Ignore());
            CreateMap<User, UpdateUserDto>()
                .ForMember(x => x.Image, x => x.Ignore());

            // post
            CreateMap<PostBlog, PostBlogViewModel>()
              .ForMember(x => x.CreatedAt, x => x.MapFrom(x => x.CreatedAt.ToString("yyyy:MM:dd")))
              .ForMember(x => x.AuthorId, x => x.MapFrom(x => x.Author.FullName));

            CreateMap<CreatePostBlogDto, PostBlog>().ForMember(x => x.Image, x => x.Ignore());
            
            CreateMap<UpdatePostBlogDto, PostBlog>().ForMember(x => x.Image, x => x.Ignore());

            CreateMap<PostBlog, UpdatePostBlogDto>().ForMember(x => x.Image, x => x.Ignore());


            //Notification   
            CreateMap<Notification, NotificationViewModel>()
            .ForMember(x => x.CreateAt, x => x.MapFrom(x => x.CreatedAt.ToString("yyyy:MM:dd")));
            CreateMap<CreateNotificationDto, Notification>();
        }
    }
}
