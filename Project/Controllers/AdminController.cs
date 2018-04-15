using AutoMapper;
using Project.Account.Services;
using Project.Core.Account;
using Project.UserProfileDomain.Repositories;
using Project.ViewModels;
using Project.ViewModels.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers {
    public class AdminController : Controller {
        readonly IUserService userService;
        readonly IUserProfileRepository userProfileRepository;
        readonly IInterestRepository interestRepository;

        public AdminController(IUserService userService, IUserProfileRepository userProfileRepository, IInterestRepository interestRepository) {
            this.userService = userService;
            this.userProfileRepository = userProfileRepository;
            this.interestRepository = interestRepository;
        }

        // GET: Admin
        public ActionResult Index() {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Dashboard() {

            var regularUsers = await userProfileRepository.GetUsersInRoleProfileAsync(StandardRoles.Normal);
            var coaches = await userProfileRepository.GetStrictInRoleUserProfilesAsync(StandardRoles.Coach);
            var interests = await interestRepository.AllAsync;

            var dashboard = new DashboardVM() {
                RegularUsers = Mapper.Map<List<UserProfileRefVM>>(regularUsers),
                Coaches = Mapper.Map<List<UserProfileRefVM>>(coaches),
                Interests = Mapper.Map<List<InterestVM>>(interests)
            };

            return View(dashboard);
        }

    }
}