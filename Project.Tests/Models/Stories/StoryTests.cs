using AutoFixture;
using AutoMapper;
using Project.StoryDomain.Models;
using Project.Tests.Utils;
using Project.ViewModels.Story;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Project.Tests.Models.Tasks {
    public class StoryTests {

        public StoryTests() {
            AutoMapperUtil.ConfigureOnce();
        }

        [Fact]
        public void ShouldHaveStoryModel() {
            var task = new Story();
        }

        [Fact]
        public void StoryComments_AreMappedTo_StoryVM() {
            var fixture = FixtureExtensions.CreateFixture();
            fixture.Customizations.Add(new ManyNavigationPropertyOmitter<Story>());
            fixture.Customizations.Add(new ManyNavigationPropertyOmitter<Comment>());
            fixture.Customizations.Add(new ManyNavigationPropertyOmitter<Group>());
            fixture.Customizations.Add(new NavigationPropertyOmitter<Comment, Comment>());

            // Arrange
            var comments = fixture.CreateMany<Comment>(2).ToList();
            var likes = fixture.CreateMany<Like>(2).ToList();

            var sut = fixture.Build<Story>()
                .With(x => x.Comments, comments)
                .With(x => x.Likes, likes)
                .Create();

            // Act
            var storyVM = Mapper.Map<StoryVM>(sut);

            // Assert
            Assert.NotNull(storyVM.Comments);
            Assert.NotEmpty(storyVM.Comments);
            Assert.All(storyVM.Comments, commentVM => {
                var expectedComment = comments.First(c => c.Id == commentVM.Id);

                Assert.Equal(expectedComment.Text, commentVM.Text);
                Assert.Equal(expectedComment.UserId, commentVM.UserId);
            });

            Assert.NotNull(storyVM.Likes);
            Assert.NotEmpty(storyVM.Likes);
            Assert.All(storyVM.Likes, likeVM => {
                var expectedLike = likes.First(c => c.Id == likeVM.Id);

                Assert.Equal(expectedLike.StoryId, likeVM.StoryId);
                Assert.Equal(expectedLike.Date, likeVM.Date);
                Assert.Equal(expectedLike.UserId, likeVM.UserId);
            });
        }

    }
}
