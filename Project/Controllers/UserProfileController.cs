using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Project.Account.Services;
using Project.StoryDomain.Repositories;
using Project.UserProfileDomain.Models;
using Project.UserProfileDomain.Repositories;
using Project.ViewModels;
using Project.ViewModels.Story;

namespace Project.Controllers {
    [RoutePrefix("UserProfile")]
    public class UserProfileController : Controller {
        readonly IUserService userService;
        readonly IUserProfileRepository userProfileRepository;
        readonly IStoryRepository storyRepository;

        public UserProfileController(IUserService userService, IUserProfileRepository userProfileRepository, IStoryRepository storyRepository) {
            this.userService = userService;
            this.userProfileRepository = userProfileRepository;
            this.storyRepository = storyRepository;
        }

        [Route("{userName?}")]
        public async Task<ViewResult> Index(string userName = null) {

            if (string.IsNullOrWhiteSpace(userName)) {
                userName = userService.GetUserName();
            }

            var userInfo = await userService.FindUserByName(userName);
            var userProfile = await userProfileRepository.GetUserProfileAsync(userInfo.Id);
            var userStories = await storyRepository.GetUserStoriesAsync(userInfo.Id);

            var viewModel = Mapper.Map<UserProfileVM>(userProfile);
            viewModel.Stories = Mapper.Map<List<StoryVM>>(userStories);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ViewResult> SaveProfile(UserProfileVM userProfileVM) {

            if (userService.GetUserId() != userProfileVM.UserId) {
                ModelState.AddModelError("UserId", "You cannot save the profile of another user.");
                return View("Index", userProfileVM);
            }

            var userProfile = Mapper.Map<UserProfile>(userProfileVM);
            userProfileRepository.InsertOrUpdate(userProfile);
            await userProfileRepository.SaveAsync();
            var updatedProfile = await userProfileRepository.GetUserProfileAsync(userService.GetUserId());

            var updatedViewModel = Mapper.Map<UserProfileVM>(updatedProfile);
            return View("Index", updatedViewModel);
        }
    }
}