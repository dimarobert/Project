using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Project.Account.Services;
using Project.Core.Account;
using Project.StoryDomain.Repositories;
using Project.UserProfileDomain.Models;
using Project.UserProfileDomain.Repositories;
using Project.ViewModels;
using Project.ViewModels.Story;
using Project.ViewModels.UserProfile;

namespace Project.Controllers {
    [RoutePrefix("UserProfile")]
    public class UserProfileController : Controller {
        readonly IUserService userService;
        readonly IUserProfileRepository userProfileRepository;
        readonly IStoryRepository storyRepository;
        readonly IInterestRepository interestRepository;
        readonly IGoalRepository goalRepository;

        public UserProfileController(IUserService userService, IUserProfileRepository userProfileRepository, IStoryRepository storyRepository, IInterestRepository interestRepository) {
            this.userService = userService;
            this.userProfileRepository = userProfileRepository;
            this.storyRepository = storyRepository;
            this.interestRepository = interestRepository;
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
            viewModel.Role = "";

            var maxRole = userInfo.Roles.DefaultIfEmpty().Max(r => {
                Enum.TryParse<StandardRoles>(r?.Role.Name, out var role);
                return role;
            });
            if (maxRole > StandardRoles.Normal)
                viewModel.Role = maxRole.ToString();

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("{userName}/Update/{updateType:enum(Project.Controllers.UserProfileUpdateType)}")]
        public async Task<ActionResult> UpdateProfile(UserProfileVM userProfileVM, string updateType) {

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

            return RedirectToAction("Index");
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

        [HttpPost]
        [Authorize, ValidateAntiForgeryToken]
        [Route("{userName}/AddInterest")]
        public async Task<ActionResult> AddInterest(UserInterestVM interest) {

            var currentUserProfile = await userProfileRepository.GetUserProfileAsync(userService.GetUserId());

            if (interest.UserProfileId != currentUserProfile.Id) {
                ModelState.AddModelError("Interest", "You cannot add interests for another user.");
                return View(interest);
            }

            var checkInterest = await interestRepository.GetAsync(i => i.Id == interest.InterestId);
            if (!checkInterest.Any()) {
                ModelState.AddModelError("Interest", "The provided interest does not exist.");
                return View(interest);
            }

            var userInterest = Mapper.Map<UserInterest>(interest);
            userInterest.State = Core.Models.ModelState.Added;

            currentUserProfile.Interests.Add(userInterest);
            userProfileRepository.InsertOrUpdateGraph(currentUserProfile);
            await userProfileRepository.SaveAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize, ValidateAntiForgeryToken]
        [Route("{userName}/AddGoal")]
        public async Task<ActionResult> AddGoal(GoalVM goal) {
            var currentUserProfile = await userProfileRepository.GetUserProfileAsync(userService.GetUserId());

            if (goal.UserProfileId != currentUserProfile.Id) {
                ModelState.AddModelError("Goal", "You cannot add goals for another user.");
                return View(goal);
            }

            var userGoal = Mapper.Map<Goal>(goal);
            userGoal.State = Core.Models.ModelState.Added;

            currentUserProfile.Goals.Add(userGoal);
            userProfileRepository.InsertOrUpdateGraph(currentUserProfile);
            await userProfileRepository.SaveAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize, ValidateAntiForgeryToken]
        [Route("{userName}/AddStep")]
        public async Task<ActionResult> AddStep(GoalVM goal) {
            var currentUserProfile = await userProfileRepository.GetUserProfileAsync(userService.GetUserId());

            if (goal.UserProfileId != currentUserProfile.Id) {
                ModelState.AddModelError("Goal", "You cannot add or update goal for another user.");
                return View(goal);
            }

            var checkGoal = await goalRepository.GetAsync(g => g.Id == goal.Id);
            if (!checkGoal.Any()) {
                ModelState.AddModelError("Goal", "The provided goal does not exist.");
                return View(goal);
            }
            var goalStep = Mapper.Map<Step>(goal);
            goalStep.State = Core.Models.ModelState.Added;

            var goalToUpdate = Mapper.Map<Goal>(goal);
            goalToUpdate.Steps.Add(goalStep);

            goalRepository.InsertOrUpdateGraph(goalToUpdate);
            await goalRepository.SaveAsync();

            return RedirectToAction("Index");
        }
    }

    public enum UserProfileUpdateType {
        Unknown = 0,
        Name,
        AboutMe,
        BirthDate
    }
}