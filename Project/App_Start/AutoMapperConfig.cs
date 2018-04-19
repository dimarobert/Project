using AutoMapper;
using Project.StoryDomain.Models;
using Project.UserProfileDomain.Models;
using Project.ViewModels;
using Project.ViewModels.Admin;
using Project.ViewModels.Story;
using Project.ViewModels.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project {
    public static class AutoMapperConfig {

        public static void Configure() {
            Mapper.Initialize(cfg => {

                cfg.CreateMap<Interest, InterestVM>().ReverseMap();
                cfg.CreateMap<UserInterestVM, UserInterest>();
                cfg.CreateMap<UserInterest, InterestVM>()
                    .ForMember(ui => ui.Id, opt => opt.MapFrom(src => src.InterestId))
                    .ForMember(ui => ui.Title, opt => opt.MapFrom(src => src.Interest.Title));

                cfg.CreateMap<Goal, GoalVM>()
                    .ForMember(g => g.UserProfileId, opt => opt.MapFrom(src => src.UserProfileId))
                    .ReverseMap();

                cfg.CreateMap<Step, StepVM>().ReverseMap();


                cfg.CreateMap<UserProfile, UserProfileVM>()
                    .ForMember(upVM => upVM.Email, opt => opt.MapFrom(src => src.User.Email))
                    .ForMember(upVM => upVM.UserName, opt => opt.MapFrom(src => src.User.UserName))
                    .ForMember(upVM => upVM.Goals, opt => opt.MapFrom(src => Mapper.Map<IList<GoalVM>>(src.Goals)))
                    .ForMember(upVM => upVM.Interests, opt => opt.MapFrom(src => Mapper.Map<IList<InterestVM>>(src.Interests)))
                    .ReverseMap();

                cfg.CreateMap<UserProfile, UserProfileRefVM>();

                cfg.CreateMap<Story, StoryVM>()
                    .ForMember(s => s.UserName, opt => opt.MapFrom(src => src.User.UserName))
                    .ReverseMap();

                cfg.CreateMap<Comment, CommentVM>().ReverseMap();
                cfg.CreateMap<Hashtag, HashtagVM>().ReverseMap();
                cfg.CreateMap<Like, LikeVM>().ReverseMap();

                // Admin VM
                cfg.CreateMap<UserProfile, UserBasicInfoVM>()
                   .ForMember(ubiVM => ubiVM.Email, opt => opt.MapFrom(src => src.User.Email))
                   .ForMember(ubiVM => ubiVM.UserName, opt => opt.MapFrom(src => src.User.UserName));

            });
        }

    }
}