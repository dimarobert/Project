using AutoMapper;
using Project.Account.Services;
using Project.UserProfileDomain.Models;
using Project.UserProfileDomain.Repositories;
using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers {
    public class UserProfileController : Controller {
        readonly IUserService userService;
        readonly IUserProfileRepository userProfileRepository;

        public UserProfileController(IUserService userService, IUserProfileRepository userProfileRepository) {
            this.userService = userService;
            this.userProfileRepository = userProfileRepository;
        }

        // GET: UserProfile
        public async Task<ViewResult> Index() {
            var userProfile = await userProfileRepository.GetUserProfileAsync(userService.GetUserId());

            var viewModel = Mapper.Map<UserProfileVM>(userProfile);
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