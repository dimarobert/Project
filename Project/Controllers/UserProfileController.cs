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
        public async Task<ActionResult> Index(string userName = null) {

            if (string.IsNullOrWhiteSpace(userName)) {
                userName = userService.GetUserName();
            }

            if (userName.Equals("index", StringComparison.OrdinalIgnoreCase))
                return RedirectToAction("Index", new { userName = "" });

            var userInfo = await userService.FindUserByNameAsync(userName);
            if (userInfo == null)
                return View("PageNotFound");

            var userProfile = await userProfileRepository.GetUserProfileAsync(userInfo.Id);
            var userStories = await storyRepository.GetUserStoriesAsync(userInfo.Id);

            var viewModel = Mapper.Map<UserProfileVM>(userProfile);
            viewModel.Stories = Mapper.Map<List<StoryVM>>(userStories);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{userName}/Update/{updateType:enum(Project.Controllers.UserProfileUpdateType)}")]
        public async Task<ViewResult> UpdateProfile(UserProfileVM userProfileVM, string updateType) {

            Enum.TryParse(updateType, out UserProfileUpdateType updateTypeEnum);

            if (updateTypeEnum == UserProfileUpdateType.Unknown) {
                ModelState.AddModelError("updateType", "Invalid update type provided. Please check the request URL parameter.");
                return View("Index", userProfileVM);
            }

            if (userService.GetUserId() != userProfileVM.UserId) {
                ModelState.AddModelError("UserId", "You cannot save the profile of another user.");
                return View("Index", userProfileVM);
            }

            var existingProfile = await userProfileRepository.GetUserProfileAsync(userService.GetUserId());

            UpdateProfileProperties(userProfileVM, existingProfile, updateTypeEnum);

            userProfileRepository.InsertOrUpdate(existingProfile);
            await userProfileRepository.SaveAsync();
            var updatedProfile = await userProfileRepository.GetUserProfileAsync(userService.GetUserId());

            var updatedViewModel = Mapper.Map<UserProfileVM>(updatedProfile);
            return View("Index", updatedViewModel);
        }

        private void UpdateProfileProperties(UserProfileVM userProfileVM, UserProfile existingProfile, UserProfileUpdateType updateTypeEnum) {
            switch (updateTypeEnum) {
                case UserProfileUpdateType.Name:
                    existingProfile.FirstName = userProfileVM.FirstName;
                    existingProfile.LastName = userProfileVM.LastName;
                    break;

                case UserProfileUpdateType.AboutMe:
                    existingProfile.AboutMe = userProfileVM.AboutMe;
                    break;
                case UserProfileUpdateType.BirthDate:
                    existingProfile.BirthDate = userProfileVM.BirthDate;
                    break;
            }
        }
    }

    public enum UserProfileUpdateType {
        Unknown = 0,
        Name,
        AboutMe,
        BirthDate
    }
}