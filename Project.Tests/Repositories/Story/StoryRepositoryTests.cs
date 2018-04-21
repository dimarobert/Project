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
            fixture.Register<IStoryContext>(() => null);

            // Assert
            Assert.Throws<ArgumentNullException>("context", () => new StoryRepository(null));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        public void Should_ReturnEmptyList_WithNullOrWhitespace_UserId(string userId) {
            var fixture = FixtureExtensions.CreateFixture();

            // Arrange
            var storyContext = fixture.FreezeDbContext<IStoryContext>();
            MockingHelpers.MockDbContextSet(storyContext, c => c.Stories, null);

            var sut = fixture.Create<StoryRepository>();

            // Act
            var userStories = sut.GetUserStories(userId);

            // Assert
            Assert.Equal(0, userStories.Count);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        public void Should_ReturnAllValues_WithSameUserId(int numberOfStories) {
            var fixture = FixtureExtensions.CreateFixture();
            fixture.Customizations.Add(new ManyNavigationPropertyOmitter<Story>());

            var userId = "1";
            // Arrange
            var stories = fixture
                .Build<Story>()
                .With(t => t.UserId, userId)
                .CreateMany(numberOfStories).AsQueryable();

            var storyContext = fixture.FreezeDbContext<IStoryContext>();
            MockingHelpers.MockDbContextSet(storyContext, c => c.Set<Story>(), stories);

            var sut = fixture.Create<StoryRepository>();

            // Act
            var userStories = sut.GetUserStories(userId);

            // Assert
            Assert.NotNull(userStories);
            Assert.Equal(numberOfStories, userStories.Count);
        }

    }
}
