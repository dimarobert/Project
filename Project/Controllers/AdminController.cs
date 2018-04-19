using AutoMapper;
using Project.Account.Services;
using Project.Core.Account;
using Project.UserProfileDomain.Repositories;
using Project.ViewModels;
using Project.ViewModels.Account;
using Project.ViewModels.Admin;
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

        public AdminController(IUserService userService, IUserProfileUnitOfWork userProfileUnitOfWork) {
            this.userService = userService;
            this.userProfileUnitOfWork = userProfileUnitOfWork;
        }

        // GET: Admin
        public ActionResult Index() {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Dashboard() {

            var regularUsers = await userProfileUnitOfWork.UserProfiles.GetUsersInRoleProfileAsync(StandardRoles.Normal);
            var coaches = await userProfileUnitOfWork.UserProfiles.GetStrictInRoleUserProfilesAsync(StandardRoles.Coach);
            var interests = await userProfileUnitOfWork.Interests.AllAsync;

            var dashboard = new DashboardVM() {
                RegularUsers = Mapper.Map<List<UserBasicInfoVM>>(regularUsers),
                Coaches = Mapper.Map<List<UserBasicInfoVM>>(coaches),
                Interests = Mapper.Map<List<InterestVM>>(interests),
                Groups = new List<GroupVM>()
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
                ModelState.AddModelError("UserId", "The user is already a member of that role.");
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

    }
}