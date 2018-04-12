using AutoMapper;
using Project.Account.Services;
using Project.UserProfileDomain.Models;
using Project.UserProfileDomain.Repositories;
using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public ViewResult Index() {
            var userProfile = userProfileRepository.GetUserProfile(userService.GetUserId());

            var viewModel = Mapper.Map<UserProfileVM>(userProfile);
            return View(viewModel);
        }

        [HttpPost]
        public ViewResult SaveProfile(UserProfileVM userProfileVM) {

            if (userService.GetUserId() != userProfileVM.UserId) {
                ModelState.AddModelError("UserId", "You cannot save the profile of another user.");
                return View("Index", userProfileVM);
            }

            var userProfile = Mapper.Map<UserProfile>(userProfileVM);
            userProfileRepository.InsertOrUpdate(userProfile);
            userProfileRepository.Save();
            var updatedProfile = userProfileRepository.GetUserProfile(userService.GetUserId());

            var updatedViewModel = Mapper.Map<UserProfileVM>(updatedProfile);
            return View("Index", updatedViewModel);
        }
    }
}