using AutoMapper;
using Project.StoryDomain.Models;
using Project.UserProfileDomain.Models;
using Project.ViewModels;
using Project.ViewModels.Story;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project {
    public static class AutoMapperConfig {

        public static void Configure() {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<UserProfile, UserProfileVM>()
                    .ForMember(up => up.Email, opt => opt.MapFrom(src => src.User.Email))
                    .ReverseMap();

                cfg.CreateMap<UserProfile, UserProfileRefVM>();

                cfg.CreateMap<Interest, InterestVM>().ReverseMap();

                cfg.CreateMap<Story, ViewModels.Story.StoryVM>().ReverseMap();
                cfg.CreateMap<Comment, CommentVM>().ReverseMap();
                cfg.CreateMap<Hashtag, HashtagVM>().ReverseMap();
                cfg.CreateMap<Like, LikeVM>().ReverseMap();

            });
        }

    }
}