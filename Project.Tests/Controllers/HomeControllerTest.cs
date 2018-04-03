using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.AspNet.Identity;
using Moq;
using Project;
using Project.Controllers;
using Project.DAL.Tasks;
using Project.Managers.Tasks;
using Project.Models.Account;
using Project.Models.Tasks;
using Project.Services.Account;
using Xunit;

namespace Project.Tests.Controllers {

    public class HomeControllerTest {

        [Fact]
        public void Index_AnonymousUser_ShouldDisplayLandingPage() {
            // Arrange
            var userService = new Mock<IUserService>();
            var taskManager = new Mock<ITaskManager>();

            HomeController controller = new HomeController(null, userService.Object, taskManager.Object);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("", result.ViewName);
        }

        [Fact]
        public void Index_AuthenticatedUser_ShouldDisplayTaskList() {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization() { ConfigureMembers = true });

            var tasks = new List<Task> { new Task { Id = 1, Title = "Test", UserId = "1" } };

            var taskManager = new Mock<ITaskManager>();
            taskManager.Setup(c => c.GetUserTasks(It.IsAny<string>())).Returns(tasks);

            var userService = new Mock<IUserService>();
            userService.Setup(i => i.IsAuthenticated).Returns(true);
            userService.Setup(i => i.GetUserId()).Returns("1");

            var controller = new HomeController(null, userService.Object, taskManager.Object);

            var result = controller.Index() as ViewResult;

            Assert.NotNull(result);
            Assert.Equal("TaskList", result.ViewName);

            Assert.IsType<List<Task>>(result.Model);
            Assert.Equal(tasks.Count, (result.Model as List<Task>).Count);
        }

        [Fact]
        public void About() {
            // Arrange
            var userService = new Mock<IUserService>();
            var taskManager = new Mock<ITaskManager>();

            HomeController controller = new HomeController(null, userService.Object, taskManager.Object);

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.Equal("Your application description page.", result.ViewBag.Message);
        }

        [Fact]
        public void Contact() {
            // Arrange
            var userService = new Mock<IUserService>();
            var taskManager = new Mock<ITaskManager>();

            HomeController controller = new HomeController(null, userService.Object, taskManager.Object);

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }
    }
}
