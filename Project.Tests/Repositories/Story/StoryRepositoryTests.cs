using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using AutoFixture;
using Moq;
using Project.Tests.Utils;
using Project.StoryDomain.DAL;
using Project.StoryDomain.Repositories;
using Project.StoryDomain.Models;

namespace Project.Tests.Repositories.StoryDomain {
    public class StoryRepositoryTests {

        [Fact]
        public void ShouldThrow_ArgumentNullExc_WithNull_StoryDbCtx() {
            var fixture = FixtureExtensions.CreateFixture();

            // Arrange
            fixture.Register<IStoryDbContext>(() => null);

            // Assert
            Assert.Throws<ArgumentNullException>("storyDbContext", () => new StoryRepository(null));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        public void Should_ReturnEmptyList_WithNullOrWhitespace_UserId(string userId) {
            var fixture = FixtureExtensions.CreateFixture();

            // Arrange
            RegisterStoryDbContext(fixture, null);

            var sut = fixture.Create<StoryRepository>();

            // Act
            var userStories = sut.GetUserStories(userId);

            // Assert
            Assert.Equal(0, userStories.Count);
        }

        [Fact]
        public void Should_ReturnEmptyList_WithNull_StoryList() {
            var fixture = FixtureExtensions.CreateFixture();

            // Arrange
            RegisterStoryDbContext(fixture, null);

            var sut = fixture.Create<StoryRepository>();

            // Act
            var userStories = sut.GetUserStories("1");

            // Assert
            Assert.Equal(0, userStories.Count);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        public void Should_ReturnAllValues_WithSameUserId(int numberOfTasks) {
            var fixture = FixtureExtensions.CreateFixture();

            var userId = "1";
            // Arrange
            var stories = fixture
                .Build<Story>()
                .With(t => t.UserId, userId).CreateMany(numberOfTasks).AsQueryable();

            RegisterStoryDbContext(fixture, stories);

            var sut = fixture.Create<StoryRepository>();

            // Act
            var userStories = sut.GetUserStories(userId);

            // Assert
            Assert.NotNull(userStories);
            Assert.Equal(numberOfTasks, userStories.Count);
        }

        private void RegisterStoryDbContext(IFixture fixture, IQueryable<Story> stories) {
            var storyDbSet = stories != null ? DbSetHelpers.GetDbSetMock(stories) : null;

            var storyDbC = new Mock<IStoryDbContext>();
            storyDbC.Setup(t => t.Stories).Returns(storyDbSet?.Object);
            fixture.Register(() => storyDbC.Object);
        }
    }
}
