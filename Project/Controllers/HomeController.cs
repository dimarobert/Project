using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project.Account.Services;
using Project.StoryDomain.Repositories;

namespace Project.Controllers {
    public class HomeController : Controller {

        readonly IUserService userService;
        readonly IStoryRepository storyRepository;

        public HomeController(IUserService userService, IStoryRepository storyRepository) {
            this.userService = userService;
            this.storyRepository = storyRepository;
        }

        public ActionResult Index() {
            if (!userService.IsAuthenticated)
                return View();

            var userStories = storyRepository.GetUserStories(userService.GetUserId());

            return View("StoryList", userStories);
        }

        [Route("About")]
        public ActionResult About() {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Route("Contact")]
        public ActionResult Contact() {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}