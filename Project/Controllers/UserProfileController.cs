using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using Project.Account.Models;
using Project.Account.Services;
using Project.Core.Account;
using Project.StoryDomain.Models;
using Project.StoryDomain.Repositories;
using Project.StoryDomain.Services;
using Project.UserProfileDomain.Models;
using Project.UserProfileDomain.Repositories;
using Project.ViewModels.Story;
using Project.ViewModels.UserProfile;

namespace Project.Controllers {
    [RoutePrefix("UserProfile")]
    public class UserProfileController : Controller {
        readonly IUserService userService;

        readonly IUserProfileUnitOfWork userProfileUOW;
        readonly IStoryUnitOfWork storyUOW;
        readonly IStoryService storyService;

        public UserProfileController(IUserService userService, IUserProfileUnitOfWork userProfileUOW, IStoryUnitOfWork storyUOW, IStoryService storyService) {
            this.userService = userService;
            this.userProfileUOW = userProfileUOW;
            this.storyUOW = storyUOW;
            this.storyService = storyService;
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

            var userProfile = await userProfileUOW.UserProfiles.GetUserProfileAsync(userInfo.Id);
            var userStories = await storyUOW.Stories.GetUserStoriesAsync(userInfo.Id);

            var availableInterests = userProfileUOW.Interests.GetAllForUser(userProfile.Id, true);

            var viewModel = Mapper.Map<UserProfileVM>(userProfile);
            viewModel.Stories = Mapper.Map<List<StoryVM>>(userStories);
            viewModel.AvailableInterests = Mapper.Map<List<InterestVM>>(availableInterests);

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

            var existingProfile = await userProfileUOW.UserProfiles.GetUserProfileAsync(userService.GetUserId());

            UpdateProfileProperties(userProfileVM, existingProfile, updateTypeEnum);

            userProfileUOW.UserProfiles.InsertOrUpdate(existingProfile);

            await userProfileUOW.CompleteAsync();

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
        public async Task<ActionResult> AddInterest(int interestId) {

            var currentUserProfile = await userProfileUOW.UserProfiles.GetUserProfileAsync(userService.GetUserId());

            var checkInterest = (await userProfileUOW.Interests.GetAsync(i => i.Id == interestId)).FirstOrDefault();
            if (checkInterest == null) {
                ModelState.AddModelError("Interest", "The provided interest does not exist.");
                return View();
            }

            var userInterest = new UserInterest {
                InterestId = interestId,
                UserProfileId = currentUserProfile.Id,
                State = Core.Models.ModelState.Added
            };

            currentUserProfile.Interests.Add(userInterest);
            userProfileUOW.UserProfiles.InsertOrUpdateGraph(currentUserProfile);

            await userProfileUOW.CompleteAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize, ValidateAntiForgeryToken]
        [Route("{userName}/AddGoal")]
        public async Task<ActionResult> AddGoal(GoalVM goal) {
            var currentUserProfile = await userProfileUOW.UserProfiles.GetUserProfileAsync(userService.GetUserId());

            if (goal.UserProfileId != currentUserProfile.Id) {
                ModelState.AddModelError("Goal", "You cannot add goals for another user.");
                return View(goal);
            }

            var userGoal = Mapper.Map<Goal>(goal);
            userGoal.State = Core.Models.ModelState.Added;

            currentUserProfile.Goals.Add(userGoal);
            userProfileUOW.UserProfiles.InsertOrUpdateGraph(currentUserProfile);

            await userProfileUOW.CompleteAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize, ValidateAntiForgeryToken]
        [Route("{userName}/AddStep")]
        public async Task<ActionResult> AddStep(GoalVM goal) {
            var currentUserProfile = await userProfileUOW.UserProfiles.GetUserProfileAsync(userService.GetUserId());

            if (goal.UserProfileId != currentUserProfile.Id) {
                ModelState.AddModelError("Goal", "You cannot add or update goal for another user.");
                return View(goal);
            }

            var checkGoal = await userProfileUOW.Goals.GetAsync(g => g.Id == goal.Id);
            if (!checkGoal.Any()) {
                ModelState.AddModelError("Goal", "The provided goal does not exist.");
                return View(goal);
            }
            var goalStep = Mapper.Map<Step>(goal);
            goalStep.State = Core.Models.ModelState.Added;

            var goalToUpdate = Mapper.Map<Goal>(goal);
            goalToUpdate.Steps.Add(goalStep);

            userProfileUOW.Goals.InsertOrUpdateGraph(goalToUpdate);

            await userProfileUOW.CompleteAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize, ValidateAntiForgeryToken]
        [Route("AddStory")]
        public async Task<ActionResult> AddStory(StoryVM story) {
            if (!ModelState.IsValid) {
                return PartialView("_AjaxValidation", "Required story fields were not filled in.");
            }

            var storyModel = Mapper.Map<Story>(story);

            storyModel.State = Core.Models.ModelState.Added;
            storyModel.Date = DateTime.Now;
            storyModel.UserId = userService.GetUserId();

            storyUOW.Stories.InsertOrUpdate(storyModel);

            var hashtags = storyService.ExtractHashtags(storyModel);
            await storyUOW.Hashtags.UpdateHashtags(hashtags);
            
            await storyUOW.CompleteAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize, ValidateAntiForgeryToken]
        [Route("Story/AddComment")]
        public async Task<ActionResult> AddStoryComment(CommentVM comment) {
            if (!ModelState.IsValid) {
                return PartialView("_AjaxValidation", "Required comment fields were not filled in.");
            }

            var commentModel = Mapper.Map<Comment>(comment);
            commentModel.Date = DateTime.Now;
            commentModel.State = Core.Models.ModelState.Added;
            commentModel.UserId = userService.GetUserId();

            storyUOW.Comments.InsertOrUpdate(commentModel);

            await storyUOW.CompleteAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize, ValidateAntiForgeryToken]
        [Route("Comment/AddReply")]
        public async Task<ActionResult> AddCommentReply(CommentVM reply) {
            if (!ModelState.IsValid) {
                return PartialView("_AjaxValidation", "Required reply fields were not filled in.");
            }

            var replyModel = Mapper.Map<Comment>(reply);
            replyModel.Date = DateTime.Now;
            replyModel.State = Core.Models.ModelState.Added;
            replyModel.UserId = userService.GetUserId();

            storyUOW.Comments.InsertOrUpdate(replyModel);

            await storyUOW.CompleteAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize, ValidateAntiForgeryToken]
        [Route("EditStory/{storyId:int}")]
        public async Task<ActionResult> EditStory(int storyId, StoryVM story) {

            var existentStory = await storyUOW.Stories.GetStoryById(storyId);

            if (existentStory == null) {
                ModelState.AddModelError("storyId", "The story that you want to edit does not exist.");
                return PartialView("_AjaxValidation", "");
            }

            if (existentStory.UserId != userService.GetUserId()) {
                ModelState.AddModelError("storyId", "You do not have the rights to edit that story.");
                return PartialView("_AjaxValidation", "");
            }

            if (existentStory.Type != story.Type)
                existentStory.Type = story.Type;
            if (!string.IsNullOrEmpty(story.Title) && existentStory.Title != story.Title)
                existentStory.Title = story.Title;
            if (!string.IsNullOrEmpty(story.Content) && existentStory.Content != story.Content)
                existentStory.Content = story.Content;

            existentStory.State = Core.Models.ModelState.Modified;

            storyUOW.Stories.InsertOrUpdate(existentStory);

            await storyUOW.CompleteAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize, ValidateAntiForgeryToken]
        [Route("DeleteStory/{storyId:int}")]
        public async Task<ActionResult> DeleteStory(int storyId) {

            var existentStory = await storyUOW.Stories.GetStoryById(storyId);

            if (existentStory == null) {
                ModelState.AddModelError("storyId", "The story that you want to delete does not exist.");
                return PartialView("_AjaxValidation", "");
            }

            if (existentStory.UserId != userService.GetUserId()) {
                ModelState.AddModelError("storyId", "You do not have the rights to delete that story.");
                return PartialView("_AjaxValidation", "");
            }

            storyUOW.Stories.Remove(existentStory);
            await storyUOW.CompleteAsync();

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