using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AutoFixture;
using AutoFixture.AutoMoq;
using Xunit;
using Moq;
using Project;
using Project.Controllers;
using Project.Tests.Utils;
using Project.StoryDomain.Repositories;
using Project.Account.Services;
using Project.StoryDomain.Models;

namespace Project.Tests.Controllers {

    public class HomeControllerTest {


        [Fact]
        public void Index_AnonymousUser_ShouldDisplayLandingPage() {
            var fixture = FixtureExtensions.CreateFixture();

            // Arrange
            var userService = fixture.Freeze<Mock<IUserService>>();
            userService.Setup(i => i.IsAuthenticated).Returns(false);

            var storyRepository = fixture.Freeze<Mock<IStoryRepository>>();

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
            fixture.Customizations.Add(new ManyNavigationPropertyOmitter<Story>());

            // Arrange
            var userStories = fixture.CreateMany<Story>(3).ToList();

            var taskRepository = fixture.Freeze<Mock<IStoryRepository>>();
            taskRepository.Setup(c => c.GetUserStories(It.IsAny<string>())).Returns(userStories);

            var userService = fixture.Freeze<Mock<IUserService>>();
            userService.Setup(i => i.IsAuthenticated).Returns(true);
            userService.Setup(i => i.GetUserId()).Returns(userStories.First().UserId);

            var controller = fixture.CreateController<HomeController>();

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("StoryList", result.ViewName);

            Assert.IsType<List<Story>>(result.Model);
            Assert.Equal(userStories.Count, (result.Model as List<Story>).Count);
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
