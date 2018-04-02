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
            HomeController controller = new HomeController();

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

            var tasks = new List<Task> { new Task { Id = 1, Title = "Test", UserId = "1" } }.AsQueryable();

            var taskData = Utils.DbSetHelpers.GetDbSetMock(tasks);

            var taskContext = new Mock<ITaskDbContext>();
            taskContext.Setup(c => c.Tasks).Returns(taskData.Object);

            var userService = new Mock<IUserService>();
            userService.Setup(i => i.IsAuthenticated).Returns(true);
            userService.Setup(i => i.GetUserId()).Returns("1");

            var controller = new HomeController(null, userService.Object, new TaskManager(taskContext.Object));

            var result = controller.Index() as ViewResult;

            Assert.NotNull(result);
            Assert.Equal("TaskList", result.ViewName);
        }

        [Fact]
        public void About() {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.Equal("Your application description page.", result.ViewBag.Message);
        }

        [Fact]
        public void Contact() {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }
    }
}
