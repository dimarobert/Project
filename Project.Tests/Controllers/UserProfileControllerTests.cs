using AutoFixture;
using Moq;
using Project.Account.Services;
using Project.Controllers;
using Project.Tests.Utils;
using Project.UserProfileDomain.Models;
using Project.UserProfileDomain.Repositories;
using Project.ViewModels;
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


        [Fact]
        public async Task Index_ShouldReturnAn_UserProfileVM() {

            var fixture = FixtureExtensions.CreateFixture();
            fixture.Customizations.Add(new ManyNavigationPropertyOmitter<UserProfile>());

            //Arange
            var upRepo = fixture.Freeze<Mock<IUserProfileRepository>>();
            var userProfile = fixture.Create<UserProfile>();

            upRepo.Setup(r => r.GetUserProfileAsync(It.IsAny<string>())).Returns(Task.FromResult(userProfile));

            var sut = fixture.CreateController<UserProfileController>();

            // Act
            var view = await sut.Index();

            // Assert
            upRepo.Verify(p => p.GetUserProfileAsync(It.IsAny<string>()));
            Assert.IsType<UserProfileVM>(view.Model);
        }

        [Fact]
        public async Task Index_ViewModel_ShouldMatch_RepositoryResult() {

            var fixture = FixtureExtensions.CreateFixture();
            fixture.Customizations.Add(new ManyNavigationPropertyOmitter<UserProfile>());

            //Arange
            var uService = fixture.Freeze<Mock<IUserService>>();
            uService.Setup(u => u.GetUserId()).Returns(() => fixture.Create<string>());

            var userProfile = fixture.Create<UserProfile>();

            var upRepo = fixture.Freeze<Mock<IUserProfileRepository>>();
            upRepo.Setup(r => r.GetUserProfileAsync(It.IsAny<string>())).Returns(Task.FromResult(userProfile));

            var sut = fixture.CreateController<UserProfileController>();

            // Act
            var view = await sut.Index();
            var model = view.Model as UserProfileVM;

            // Assert
            Assert.Equal(userProfile.Id, model.Id);
            Assert.Equal(userProfile.UserId, model.UserId);
            Assert.Equal(userProfile.AboutMe, model.AboutMe);
            Assert.Equal(userProfile.BirthDate, model.BirthDate);
            Assert.Equal(userProfile.User.UserName, model.UserName);
            Assert.Equal(userProfile.User.Email, model.Email);
            Assert.Equal(userProfile.FirstName, model.FirstName);
            Assert.Equal(userProfile.LastName, model.LastName);
        }


        [Fact]
        public async Task SaveProfile_ShouldReturn_IndexView() {

            var fixture = FixtureExtensions.CreateFixture();
            fixture.Customizations.Add(new ManyNavigationPropertyOmitter<UserProfile>());

            //Arange
            var upRepo = fixture.Freeze<Mock<IUserProfileRepository>>();

            var sut = fixture.CreateController<UserProfileController>();
            var userProfileVM = fixture.Create<UserProfileVM>();

            // Act
            var view = await sut.SaveProfile(userProfileVM);

            // Assert
            Assert.Equal("Index", view.ViewName);
        }

        [Fact]
        public async Task SaveProfile_ShouldReturnModel_UserProfileVM() {

            var fixture = FixtureExtensions.CreateFixture();
            fixture.Customizations.Add(new ManyNavigationPropertyOmitter<UserProfile>());

            //Arange
            var upRepo = fixture.Freeze<Mock<IUserProfileRepository>>();

            var sut = fixture.CreateController<UserProfileController>();
            var userProfileVM = fixture.Create<UserProfileVM>();

            // Act
            var view = await sut.SaveProfile(userProfileVM);

            // Assert
            Assert.IsType<UserProfileVM>(view.Model);
        }

        [Fact]
        public async Task SaveProfile_ShouldCall_UserProfileRepo_InsertOrUpdate_ForValidUserId() {

            var fixture = FixtureExtensions.CreateFixture();
            fixture.Customizations.Add(new ManyNavigationPropertyOmitter<UserProfile>());

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
            upRepo.Verify(r => r.InsertOrUpdate(It.IsAny<UserProfile>()));
        }

        [Fact]
        public async Task SaveProfile_ShouldNot_UpdateProfileForAnotherUser() {

            var fixture = FixtureExtensions.CreateFixture();
            fixture.Customizations.Add(new ManyNavigationPropertyOmitter<UserProfile>());

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
            Assert.Equal(userProfileVM, view.Model);
        }

        [Fact]
        public async Task SaveProfile_ShouldReturn_TheUpdatedVM() {

            var fixture = FixtureExtensions.CreateFixture();
            fixture.Customizations.Add(new ManyNavigationPropertyOmitter<UserProfile>());

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
