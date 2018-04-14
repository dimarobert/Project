using AutoFixture;
using Moq;
using Project.Account.Models;
using Project.Account.Services;
using Project.Controllers;
using Project.StoryDomain.Models;
using Project.StoryDomain.Repositories;
using Project.Tests.Utils;
using Project.UserProfileDomain.Models;
using Project.UserProfileDomain.Repositories;
using Project.ViewModels;
using Project.ViewModels.Story;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xunit;

namespace Project.Tests.Controllers {
    public class UserProfileControllerTests {

        public UserProfileControllerTests() {
            AutoMapperUtil.ConfigureOnce();
        }

        void AddCustomizations(IFixture fixture) {
            fixture.Customizations.Add(new ManyNavigationPropertyOmitter<UserProfile>());
            fixture.Customizations.Add(new ManyNavigationPropertyOmitter<Story>());
            fixture.Customizations.Add(new ManyNavigationPropertyOmitter<StoryVM>());
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("123")]
        public async Task Index_ShouldReturnAn_UserProfileVM(string userName) {

            var fixture = FixtureExtensions.CreateFixture();
            AddCustomizations(fixture);

            //Arange
            var userProfile = fixture.Create<UserProfile>();

            var upRepo = fixture.Freeze<Mock<IUserProfileRepository>>();
            upRepo.Setup(r => r.GetUserProfileAsync(It.IsAny<string>())).Returns(Task.FromResult(userProfile));

            var sut = fixture.CreateController<UserProfileController>();

            // Act
            var view = await sut.Index(userName);

            // Assert
            Assert.IsType<UserProfileVM>(view.Model);
        }

        [Fact]
        public async Task Index_CalledWithNoUserName_ShouldReturn_CurrentUserProfileVM() {

            var fixture = FixtureExtensions.CreateFixture();
            AddCustomizations(fixture);

            //Arange
            var currentUserInfo = fixture.Create<UserInfo>();

            var uService = fixture.Freeze<Mock<IUserService>>();
            uService.Setup(u => u.GetUserName()).Returns(currentUserInfo.UserName);
            uService.Setup(u => u.FindUserByName(It.Is<string>(_userName => _userName == currentUserInfo.UserName))).Returns(Task.FromResult(currentUserInfo));

            var currentUserProfile = fixture.Build<UserProfile>()
                .With(up => up.User, currentUserInfo)
                .With(up => up.UserId, currentUserInfo.Id)
                .Create();

            var upRepo = fixture.Freeze<Mock<IUserProfileRepository>>();
            upRepo.Setup(r => r.GetUserProfileAsync(It.Is<string>(_userId => _userId == currentUserInfo.Id))).Returns(Task.FromResult(currentUserProfile));

            var sut = fixture.CreateController<UserProfileController>();

            // Act
            var view = await sut.Index();
            var model = view.Model as UserProfileVM;

            // Assert
            uService.Verify(u => u.GetUserName(), Times.Once());
            uService.Verify(u => u.FindUserByName(It.Is<string>(_userName => _userName == currentUserInfo.UserName)), Times.Once(), "FindUserByName was not called or called with wrong parameter.");
            upRepo.Verify(r => r.GetUserProfileAsync(It.Is<string>(_userId => _userId == currentUserInfo.Id)), Times.Once(), "GetUserProfileAsync was not called or called with wrong parameter.");
            Assert.IsType<UserProfileVM>(view.Model);
            Assert.Equal(currentUserProfile.User.UserName, model.UserName);
            Assert.Equal(currentUserProfile.UserId, model.UserId);
        }

        [Fact]
        public async Task Index_ViewModel_ShouldMatch_RepositoryResult() {

            var fixture = FixtureExtensions.CreateFixture();
            AddCustomizations(fixture);

            //Arange
            var uService = fixture.Freeze<Mock<IUserService>>();
            uService.Setup(u => u.GetUserId()).Returns(() => fixture.Create<string>());

            var profileFromRepository = fixture.Create<UserProfile>();

            var upRepo = fixture.Freeze<Mock<IUserProfileRepository>>();
            upRepo.Setup(r => r.GetUserProfileAsync(It.IsAny<string>())).Returns(Task.FromResult(profileFromRepository));

            var sut = fixture.CreateController<UserProfileController>();

            // Act
            var view = await sut.Index(profileFromRepository.User.UserName);
            var model = view.Model as UserProfileVM;

            // Assert
            Assert.Equal(profileFromRepository.Id, model.Id);
            Assert.Equal(profileFromRepository.UserId, model.UserId);
            Assert.Equal(profileFromRepository.AboutMe, model.AboutMe);
            Assert.Equal(profileFromRepository.BirthDate, model.BirthDate);
            Assert.Equal(profileFromRepository.User.UserName, model.UserName);
            Assert.Equal(profileFromRepository.User.Email, model.Email);
            Assert.Equal(profileFromRepository.FirstName, model.FirstName);
            Assert.Equal(profileFromRepository.LastName, model.LastName);
        }

        [Theory]
        [InlineData(null, 0)]
        [InlineData("aaa", 0)]
        [InlineData(null, 1)]
        [InlineData("aaa", 1)]
        [InlineData(null, 10)]
        [InlineData("aaa", 10)]
        public async Task Index_ShouldQueryUserStories(string userName, int numberOfStories) {
            var fixture = FixtureExtensions.CreateFixture();
            AddCustomizations(fixture);

            // Arrange
            var userStories = fixture.CreateMany<Story>(numberOfStories).ToList();

            var storyRepo = fixture.Freeze<Mock<IStoryRepository>>();
            storyRepo.Setup(r => r.GetUserStoriesAsync(It.IsAny<string>())).Returns(Task.FromResult(userStories as IList<Story>));

            var sut = fixture.CreateController<UserProfileController>();

            // Act
            var view = await sut.Index(userName);
            var model = view.Model as UserProfileVM;

            // Assert
            Assert.NotNull(model.Stories);
            if (!userStories.Any())
                Assert.Empty(model.Stories);
            else {
                Assert.All(model.Stories, storyVM => {
                    var expectedStory = userStories.First(s => s.Id == storyVM.Id);

                    Assert.Equal(expectedStory.Title, storyVM.Title);
                    Assert.Equal(expectedStory.User.UserName, storyVM.UserName);
                    Assert.Equal(expectedStory.UserId, storyVM.UserId);
                    Assert.Equal(expectedStory.User.Email, storyVM.UserEmail);
                });
            }
        }


        [Fact]
        public async Task SaveProfile_ShouldReturn_IndexView() {

            var fixture = FixtureExtensions.CreateFixture();
            AddCustomizations(fixture);

            //Arange
            var upRepo = fixture.Freeze<Mock<IUserProfileRepository>>();

            var sut = fixture.CreateController<UserProfileController>();
            var userProfileVM = fixture.Create<UserProfileVM>();

            // Act
            var view = await sut.SaveProfile(userProfileVM);

            // Assert
            Assert.Equal("Index", view.ViewName);
        }

        [Theory]
        [InlineData("123", "321")]
        [InlineData("123", "123")]
        public async Task SaveProfile_ShouldReturnModel_UserProfileVM(string currentUserId, string savedUserId) {

            var fixture = FixtureExtensions.CreateFixture();
            AddCustomizations(fixture);

            //Arange
            var uService = fixture.Freeze<Mock<IUserService>>();
            uService.Setup(s => s.GetUserId()).Returns(currentUserId);

            var sut = fixture.CreateController<UserProfileController>();
            var userProfileVM = fixture.Build<UserProfileVM>()
                .With(p => p.UserId, savedUserId)
                .Create();

            // Act
            var view = await sut.SaveProfile(userProfileVM);

            // Assert
            Assert.IsType<UserProfileVM>(view.Model);
        }

        [Fact]
        public async Task SaveProfile_ShouldCall_UserProfileRepo_InsertOrUpdate_ForValidUserId() {

            var fixture = FixtureExtensions.CreateFixture();
            AddCustomizations(fixture);

            //Arange
            var userId = fixture.Create<string>();

            var uService = fixture.Freeze<Mock<IUserService>>();
            uService.Setup(u => u.GetUserId()).Returns(userId);

            var upRepo = fixture.Freeze<Mock<IUserProfileRepository>>();
            UserProfile userProfile;

            upRepo.Setup(r => r.InsertOrUpdate(It.IsAny<UserProfile>())).Callback<UserProfile>(profile => userProfile = profile);

            var sut = fixture.CreateController<UserProfileController>();
            var userProfileVM = fixture.Build<UserProfileVM>()
                .With(p => p.UserId, userId)
                .Create();

            // Act
            var view = await sut.SaveProfile(userProfileVM);

            // Assert
            upRepo.Verify(r => r.InsertOrUpdate(It.Is<UserProfile>(_profile => _profile.UserId == userId)));
        }

        [Fact]
        public async Task SaveProfile_ShouldNot_UpdateProfileForAnotherUser() {

            var fixture = FixtureExtensions.CreateFixture();
            AddCustomizations(fixture);

            //Arange
            var anotherUserId = fixture.Create<string>();
            var loggedInUserId = fixture.Create<string>();
            var uService = fixture.Freeze<Mock<IUserService>>();
            uService.Setup(u => u.GetUserId()).Returns(loggedInUserId);

            var upRepo = fixture.Freeze<Mock<IUserProfileRepository>>();

            upRepo.Setup(r => r.InsertOrUpdate(It.IsAny<UserProfile>()))
                .Throws(new Exception("The user should not be saved. IUserProfileRepository.InsertOrUpdate should not be called."));

            var sut = fixture.CreateController<UserProfileController>();
            var userProfileVM = fixture.Build<UserProfileVM>()
                .With(profile => profile.UserId, anotherUserId)
                .Create();

            // Act
            var view = await sut.SaveProfile(userProfileVM);

            // Assert
            Assert.False(sut.ModelState.IsValid);
            Assert.True(sut.ModelState.ContainsKey("UserId"));
            Assert.Equal(userProfileVM, view.Model);
        }

        [Fact]
        public async Task SaveProfile_ShouldReturn_TheUpdatedVM() {

            var fixture = FixtureExtensions.CreateFixture();
            AddCustomizations(fixture);

            //Arange
            var loggedInUserId = fixture.Create<string>();

            var uService = fixture.Freeze<Mock<IUserService>>();
            uService.Setup(u => u.GetUserId()).Returns(loggedInUserId);

            UserProfile savedUserProfile = null;

            var upRepo = fixture.Freeze<Mock<IUserProfileRepository>>();
            upRepo.Setup(r => r.InsertOrUpdate(It.IsAny<UserProfile>()))
                .Callback<UserProfile>(p => savedUserProfile = p);
            upRepo.Setup(r => r.GetUserProfileAsync(It.IsAny<string>()))
                .Returns(() => Task.FromResult(savedUserProfile));

            var sut = fixture.CreateController<UserProfileController>();
            var expectedVM = fixture.Build<UserProfileVM>()
                .With(profile => profile.UserId, loggedInUserId)
                .Create();

            // Act
            var view = await sut.SaveProfile(expectedVM);

            // Assert
            upRepo.Verify(r => r.SaveAsync());
            upRepo.Verify(r => r.GetUserProfileAsync(It.IsAny<string>()));

            Assert.IsType<UserProfileVM>(view.Model);

            var model = view.Model as UserProfileVM;
            Assert.Equal(expectedVM.Id, model.Id);
            Assert.Equal(expectedVM.UserId, model.UserId);
            Assert.Equal(expectedVM.AboutMe, model.AboutMe);
            Assert.Equal(expectedVM.BirthDate, model.BirthDate);
            Assert.Equal(expectedVM.UserName, model.UserName);
            Assert.Equal(expectedVM.Email, model.Email);
            Assert.Equal(expectedVM.FirstName, model.FirstName);
            Assert.Equal(expectedVM.LastName, model.LastName);
        }
    }
}
