using AutoMapper;
using Project.StoryDomain.Repositories;
using Project.UserProfileDomain.Repositories;
using Project.ViewModels.Story;
using Project.ViewModels.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers
{
    public class GroupController : Controller
    {
        readonly IUserProfileUnitOfWork userProfileUnitOfWork;
        readonly IStoryUnitOfWork storyUnitOfWork;

        [HttpPost]
        [Authorize, ValidateAntiForgeryToken]
        public ActionResult Index(int groupId)
        {
            var members = new List<UserProfileRefVM>();
            var givingAdviceStories = new List<StoryVM>();
            var askingAdviceStories = new List<StoryVM>();
            var regularStories = new List<StoryVM>();

            var group = new GroupVM() {
                Members = Mapper.Map<List<UserProfileRefVM>>(members),
                RegularStories = Mapper.Map<List<StoryVM>>(askingAdviceStories),
                AskingAdviceStories = Mapper.Map<List<StoryVM>>(askingAdviceStories),
                GivingAdviceStories = Mapper.Map<List<StoryVM>>(givingAdviceStories),
            };
            return View();
        }
    }
}