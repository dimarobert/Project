using AutoMapper;
using Project.Account.Services;
using Project.Core.Account;
using Project.StoryDomain.Models;
using Project.StoryDomain.Repositories;
using Project.UserProfileDomain.Models;
using Project.UserProfileDomain.Repositories;
using Project.ViewModels;
using Project.ViewModels.Account;
using Project.ViewModels.Admin;
using Project.ViewModels.Story;
using Project.ViewModels.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers {
    public class AdminController : Controller {
        readonly IUserService userService;
        readonly IUserProfileUnitOfWork userProfileUnitOfWork;
        readonly IStoryUnitOfWork storyUnitOfWork;

        public AdminController(IUserService userService, IUserProfileUnitOfWork userProfileUnitOfWork, IStoryUnitOfWork storyUnitOfWork) {
            this.userService = userService;
            this.userProfileUnitOfWork = userProfileUnitOfWork;
            this.storyUnitOfWork = storyUnitOfWork;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Index() {
            return RedirectToAction("Dashboard");
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Dashboard() {

            var regularUsers = await userProfileUnitOfWork.UserProfiles.GetStrictInRoleUserProfilesAsync(StandardRoles.Normal);
            var coaches = await userProfileUnitOfWork.UserProfiles.GetUsersInRoleProfileAsync(StandardRoles.Coach);
            var admins = await userProfileUnitOfWork.UserProfiles.GetStrictInRoleUserProfilesAsync(StandardRoles.Admin);

            var interests = await userProfileUnitOfWork.Interests.AllAsync;
            var groups = await storyUnitOfWork.Groups.AllAsync;
            var hashtags = await storyUnitOfWork.Hashtags.AllAsync;

            var dashboard = new DashboardVM() {
                RegularUsers = Mapper.Map<List<UserBasicInfoVM>>(regularUsers),
                Coaches = Mapper.Map<List<UserBasicInfoVM>>(coaches),
                Admins = Mapper.Map<List<UserBasicInfoVM>>(admins),
                Interests = Mapper.Map<List<InterestVM>>(interests),
                Groups = Mapper.Map<List<GroupVM>>(groups),
                Hashtags = Mapper.Map<List<HashtagVM>>(hashtags)
            };

            return View(dashboard);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GrantRole(GrantRoleVM grantRoleModel) {

            var uInfo = await userService.FindUserByIdAsync(grantRoleModel.UserId);

            if (uInfo == null) {
                ModelState.AddModelError("UserId", "The provided user does not exist.");
                return PartialView("_AjaxValidation", "Could not add the user to the role.");
            }

            if (userService.IsInRole(grantRoleModel.UserId, grantRoleModel.RoleName)) {
                ModelState.AddModelError("UserAlreadyInRole", "The user is already a member of that role.");
                return PartialView("_AjaxValidation", "Could not add the user to the role.");
            }

            var result = await userService.AddToRoleAsync(grantRoleModel.UserId, grantRoleModel.RoleName);

            if (!result.Succeded) {
                foreach (var err in result.Errors)
                    ModelState.AddModelError("GrantRoleAction", err);
                return PartialView("_AjaxValidation", "Could not add the user to the role.");
            }

            return Json(new { location = Url.Action("Dashboard") });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> RevokeRole(GrantRoleVM revokeRoleModel) {

            var uInfo = await userService.FindUserByIdAsync(revokeRoleModel.UserId);

            if (uInfo == null) {
                ModelState.AddModelError("UserId", "The provided user does not exist.");
                return PartialView("_AjaxValidation", "Could not remove the user from the role.");
            }

            if (!userService.IsInRole(revokeRoleModel.UserId, revokeRoleModel.RoleName)) {
                ModelState.AddModelError("RoleName", "The user is not a member of that role.");
                return PartialView("_AjaxValidation", "Could not remove the user from the role.");
            }

            var result = await userService.RemoveFromRoleAsync(revokeRoleModel.UserId, revokeRoleModel.RoleName);

            if (!result.Succeded) {
                foreach (var err in result.Errors)
                    ModelState.AddModelError($"RevokeRoleAction", err);

                return PartialView("_AjaxValidation", "Could not remove the user from the role.");
            }

            return Json(new { location = Url.Action("Dashboard") });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateGroup(GroupVM group) {

            if (!ModelState.IsValid) {
                return PartialView("_AjaxValidation", "Could not create the group.");
            }

            var existingGroup = await storyUnitOfWork.Groups.FindByTitleAsync(group.Title);
            if (existingGroup != null) {
                ModelState.AddModelError("GroupTitle", "A group with that title already exists.");
                return PartialView("_AjaxValidation", "Could not create the group.");
            }

            var groupInfo = Mapper.Map<Group>(group);

            storyUnitOfWork.Groups.InsertOrUpdate(groupInfo);

            var existingHashtag = await storyUnitOfWork.Hashtags.FindByValueAsync(group.Title);
            if (existingHashtag != null) {
                storyUnitOfWork.Hashtags.Remove(existingHashtag);
            }
            await storyUnitOfWork.CompleteAsync();

            return Json(new { location = Url.Action("Dashboard") });
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateInterest(InterestVM interest) {

            var existingInterest = userProfileUnitOfWork.Interests.Get(i => i.Title == interest.Title).FirstOrDefault();

            if (existingInterest != null)
                return RedirectToAction("Dashboard");


            var interestModel = Mapper.Map<Interest>(interest);

            userProfileUnitOfWork.Interests.InsertOrUpdate(interestModel);
            await userProfileUnitOfWork.CompleteAsync();

            var group = new Group {
                InterestId = interestModel.Id,
                Title = interestModel.Title
            };

            storyUnitOfWork.Groups.InsertOrUpdate(group);
            await storyUnitOfWork.CompleteAsync();

            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> BanUser(BanUserVM bannedUser) {

            if (!ModelState.IsValid) {
                return PartialView("_AjaxValidation", "Invalid model.");
            }
            if (bannedUser.BanUntil < DateTime.Now) {
                ModelState.AddModelError("BanUntil", "Must be greater than the current date.");
                return PartialView("_AjaxValidation", "Invalid model.");
            }

            var user = await userProfileUnitOfWork.UserProfiles.GetAsync(bannedUser.UserProfileId);

            if (user == null) {
                ModelState.AddModelError("UserProfileId", "The user does not exist.");
                return PartialView("_AjaxValidation", "Invalid model.");
            }

            user.BannedUntil = bannedUser.BanUntil;

            userProfileUnitOfWork.UserProfiles.InsertOrUpdate(user);

            await userProfileUnitOfWork.CompleteAsync();

            return RedirectToAction("Dashboard");
        }
    }
}