using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Project.DAL.Tasks;
using Project.Repositories.Tasks;
using Project.Models.Account;
using Project.Services.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers {
    public class HomeController : Controller {

        //readonly ApplicationUserManager userManager;
        readonly IUserService userService;
        readonly ITaskRepository taskRepository;

        public HomeController(/*ApplicationUserManager userManager,*/ IUserService userService, ITaskRepository taskManager) {
            //this.userManager = userManager;
            this.userService = userService;
            this.taskRepository = taskManager;
        }

        public ActionResult Index() {
            if (!userService.IsAuthenticated)
                return View();

            var tasks = taskRepository.GetUserTasks(userService.GetUserId());

            return View("TaskList", tasks);
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

        //private UserInfo GetCurrentUser() {
        //    return UserManager.FindById(User.Identity.GetUserId());
        //}
    }
}