using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Project.DAL.Tasks;
using Project.Managers.Tasks;
using Project.Models.Account;
using Project.Services.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers {
    public class HomeController : Controller {

        ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        readonly IUserService userService;
        readonly ITaskManager taskManager;

        public HomeController(ApplicationUserManager userManager, IUserService userService, ITaskManager taskManager) {
            UserManager = userManager;
            this.userService = userService;
            this.taskManager = taskManager;
        }

        public ActionResult Index() {
            if (!userService.IsAuthenticated)
                return View();

            var tasks = taskManager.GetUserTasks(userService.GetUserId());

            return View("TaskList", tasks);
        }

        public ActionResult About() {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact() {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //private UserInfo GetCurrentUser() {
        //    return UserManager.FindById(User.Identity.GetUserId());
        //}
    }
}