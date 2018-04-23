using AutoMapper;
using Project.Account.Services;
using Project.Core.Enums;
using Project.StoryDomain.Repositories;
using Project.UserProfileDomain.Repositories;
using Project.ViewModels.Admin;
using Project.ViewModels.Story;
using Project.ViewModels.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers
{
    public class GroupController : Controller
    {
        readonly IUserService userService;
        readonly IUserProfileUnitOfWork userProfileUnitOfWork;
        readonly IStoryUnitOfWork storyUnitOfWork;

        public GroupController(IUserService userService, IUserProfileUnitOfWork userProfileUnitOfWork, IStoryUnitOfWork storyUnitOfWork) {
            this.userService = userService;
            this.userProfileUnitOfWork = userProfileUnitOfWork;
            this.storyUnitOfWork = storyUnitOfWork;
        }

        [HttpPost]
        [Authorize, ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(int groupId)
        {
            var group = await storyUnitOfWork.Groups.GetGroupByIdAsync(groupId);

            if(group == null) {
                ModelState.AddModelError("Group", "The provided interest group does not exist.");
                return RedirectToAction("Index", new { groupId = 0});
            }

            var givingAdviceStories = await storyUnitOfWork.Stories.GetUnpromotedStoriesByGroupAndTypeAsync(group.Id, StoryType.GivingAdvice);
            var askingAdviceStories = await storyUnitOfWork.Stories.GetUnpromotedStoriesByGroupAndTypeAsync(group.Id, StoryType.AskingAdvice);
            var regularStories = await storyUnitOfWork.Stories.GetUnpromotedStoriesByGroupAndTypeAsync(group.Id, StoryType.Regular);
            var pGivingAdviceStories = await storyUnitOfWork.Stories.GetPromotedStoriesByGroupAndTypeAsync(group.Id, StoryType.GivingAdvice);
            var pAskingAdviceStories = await storyUnitOfWork.Stories.GetPromotedStoriesByGroupAndTypeAsync(group.Id, StoryType.AskingAdvice);
            var pRegularStories = await storyUnitOfWork.Stories.GetPromotedStoriesByGroupAndTypeAsync(group.Id, StoryType.Regular);

            var groupVM = new GroupVM() {
                Members = Mapper.Map<List<UserBasicInfoVM>>(group.Members),
                RegularStories = Mapper.Map<List<StoryVM>>(regularStories),
                AskingAdviceStories = Mapper.Map<List<StoryVM>>(askingAdviceStories),
                GivingAdviceStories = Mapper.Map<List<StoryVM>>(givingAdviceStories),
                PromotedRegularStories = Mapper.Map<List<StoryVM>>(pRegularStories),
                PromotedAskingAdviceStories = Mapper.Map<List<StoryVM>>(pAskingAdviceStories),
                PromotedGivingAdviceStories = Mapper.Map<List<StoryVM>>(pGivingAdviceStories),
            };
            return View(groupVM);
        }
    }
}