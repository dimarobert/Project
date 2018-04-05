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
using Project.Tests.Utils;
using Xunit;

namespace Project.Tests.Controllers {

    public class HomeControllerTest {


        [Fact]
        public void Index_AnonymousUser_ShouldDisplayLandingPage() {
            var fixture = FixtureExtensions.CreateFixture();

            // Arrange
            var userService = fixture.Freeze<Mock<IUserService>>();
            userService.Setup(i => i.IsAuthenticated).Returns(false);

            var taskManager = fixture.Freeze<Mock<ITaskManager>>();

            var controller = fixture.CreateController<HomeController>();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("", result.ViewName);
        }

        [Fact]
        public void Index_AuthenticatedUser_ShouldDisplayTaskList() {
            var fixture = FixtureExtensions.CreateFixture();

            // Arrange
            var tasks = fixture.CreateMany<Task>(3).ToList();

            var taskManager = fixture.Freeze<Mock<ITaskManager>>();
            taskManager.Setup(c => c.GetUserTasks(It.IsAny<string>())).Returns(tasks);

            var userService = fixture.Freeze<Mock<IUserService>>();
            userService.Setup(i => i.IsAuthenticated).Returns(true);
            userService.Setup(i => i.GetUserId()).Returns(tasks.First().UserId);

            var controller = fixture.CreateController<HomeController>();

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("TaskList", result.ViewName);

            Assert.IsType<List<Task>>(result.Model);
            Assert.Equal(tasks.Count, (result.Model as List<Task>).Count);
        }

        [Fact]
        public void About() {
            var fixture = FixtureExtensions.CreateFixture();

            // Arrange
            var controller = fixture.CreateController<HomeController>();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.Equal("Your application description page.", result.ViewBag.Message);
        }

        [Fact]
        public void Contact() {
            var fixture = FixtureExtensions.CreateFixture();

            // Arrange
            var controller = fixture.CreateController<HomeController>();

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }
    }
}
