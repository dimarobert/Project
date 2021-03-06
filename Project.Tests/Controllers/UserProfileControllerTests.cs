﻿using AutoFixture;
using Moq;
using Project.Account.Models;
using Project.Account.Services;
using Project.Controllers;
using Project.Core.Account;
using Project.StoryDomain.Models;
using Project.StoryDomain.Repositories;
using Project.Tests.Utils;
using Project.UserProfileDomain.Models;
using Project.UserProfileDomain.Repositories;
using Project.ViewModels;
using Project.ViewModels.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xunit;

namespace Project.Tests.Controllers {
    public class UserProfileControllerTests {

        AutoMapper.IMapper mapper;

        public UserProfileControllerTests() {
            AutoMapperUtil.ConfigureOnce();

            var config = new AutoMapper.MapperConfiguration(cfg => {
                cfg.CreateMap<UserProfile, UserProfile>();
            });
            mapper = config.CreateMapper();
        }

        void AddCustomizations(IFixture fixture) {
            fixture.Customizations.Add(new ManyNavigationPropertyOmitter<UserProfile>());
            fixture.Customizations.Add(new ManyNavigationPropertyOmitter<Story>());
            fixture.Customizations.Add(new ManyNavigationPropertyOmitter<UserProfileVM>());
            fixture.Customizations.Add(new ManyNavigationPropertyOmitter<Group>());
        }

        [Fact]
        public async Task Index_WithInvalidUserName_ShouldDispaly404() {
            var fixture = FixtureExtensions.CreateFixture();
            AddCustomizations(fixture);

            // Arrange
            var userName = fixture.Create<string>();

            var uService = fixture.Freeze<Mock<IUserService>>();
            uService.Setup(s => s.FindUserByNameAsync(It.IsAny<string>())).Returns(Task.FromResult<UserInfo>(null));

            var sut = fixture.CreateController<UserProfileController>();

            // Act

            var action = await sut.Index(userName);
            var view = action as ViewResult;
            // Assert
            Assert.IsType<ViewResult>(action);

            Assert.Equal("PageNotFound", view.ViewName);
        }

        [Theory]
        [InlineData("Index")]
        [InlineData("index")]
        public async Task Index_WithActionNameAsUserName_ShouldRedirectTo_CurrentUser(string userName) {
            var fixture = FixtureExtensions.CreateFixture();
            AddCustomizations(fixture);

            // Arrange
            var uService = fixture.Freeze<Mock<IUserService>>();
            uService.Setup(s => s.FindUserByNameAsync(It.IsAny<string>())).Returns(Task.FromResult<UserInfo>(null));

            var sut = fixture.CreateController<UserProfileController>();

            // Act

            var action = await sut.Index(userName);
            var redirectAction = action as RedirectToRouteResult;
            // Assert
            Assert.IsType<RedirectToRouteResult>(action);

            Assert.True(redirectAction.RouteValues.ContainsKey("action"));
            Assert.Equal("Index", redirectAction.RouteValues["action"]);
            Assert.True(redirectAction.RouteValues.ContainsKey("userName"));
            Assert.Equal("", redirectAction.RouteValues["userName"]);
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
            var view = await sut.Index(userName) as ViewResult;

            // Assert
            Assert.IsType<UserProfileVM>(view.Model);
        }

        [Fact]
        public async Task Index_CalledWithNoUserName_ShouldReturn_CurrentUserProfileVM() {

            var fixture = FixtureExtensions.CreateFixture();
            AddCustomizations(fixture);

            //Arange
            var currentUserInfo = fixture.CreateUser(null);

            var uService = fixture.Freeze<Mock<IUserService>>();
            uService.Setup(u => u.GetUserName()).Returns(currentUserInfo.UserName);
            uService.Setup(u => u.FindUserByNameAsync(It.Is<string>(_userName => _userName == currentUserInfo.UserName))).Returns(Task.FromResult(currentUserInfo));

            var currentUserProfile = fixture.Build<UserProfile>()
                .With(up => up.User, currentUserInfo)
                .With(up => up.UserId, currentUserInfo.Id)
                .Create();

            var upRepo = fixture.Freeze<Mock<IUserProfileRepository>>();
            upRepo.Setup(r => r.GetUserProfileAsync(It.Is<string>(_userId => _userId == currentUserInfo.Id))).Returns(Task.FromResult(currentUserProfile));

            var sut = fixture.CreateController<UserProfileController>();

            // Act
            var view = await sut.Index() as ViewResult;
            var model = view.Model as UserProfileVM;

            // Assert
            uService.Verify(u => u.GetUserName(), Times.Once());
            uService.Verify(u => u.FindUserByNameAsync(It.Is<string>(_userName => _userName == currentUserInfo.UserName)), Times.Once(), "FindUserByName was not called or called with wrong parameter.");
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
            var view = await sut.Index(profileFromRepository.User.UserName) as ViewResult;
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
            var view = await sut.Index(userName) as ViewResult;
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

        [Theory]
        [InlineData(StandardRoles.Admin)]
        [InlineData(StandardRoles.Coach)]
        [InlineData(StandardRoles.Normal)]
        public async Task Index_ShouldReturn_CorrectUserRole(StandardRoles role) {
            var fixture = FixtureExtensions.CreateFixture();
            AddCustomizations(fixture);

            // Arrange
            var userName = fixture.Create<string>();

            var uInfo = fixture.CreateUser(
                fixture.CreateMany<StandardRoles>()
                    .Where(r => r < role)
                    .Distinct()
                    .Concat(new[] { role })
                    .ToList()
            );

            var uService = fixture.Freeze<Mock<IUserService>>();
            uService.Setup(s => s.FindUserByNameAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(uInfo));

            var sut = fixture.CreateController<UserProfileController>();

            // Act
            var view = await sut.Index(userName) as ViewResult;
            var model = view.Model as UserProfileVM;

            // Assert
            if (role == StandardRoles.Normal)
                Assert.Equal("", model.Role);
            else
                Assert.Equal(role.ToString(), model.Role);

        }

        [Fact]
        public void UpdateProfile_ShouldBeAccessible_ToLoggedUsersOnly() {

            var controllerType = typeof(UserProfileController);
            var dashboardMethodType = controllerType.GetMethod("UpdateProfile");
            Assert.NotNull(dashboardMethodType);

            var authAttrib = dashboardMethodType.CustomAttributes.FirstOrDefault(i => i.AttributeType == typeof(AuthorizeAttribute));
            Assert.NotNull(authAttrib);
        }

        [Fact]
        public async Task UpdateProfile_ShouldReturn_IndexView() {

            var fixture = FixtureExtensions.CreateFixture();
            AddCustomizations(fixture);

            //Arange
            var upRepo = fixture.Freeze<Mock<IUserProfileRepository>>();

            var sut = fixture.CreateController<UserProfileController>();
            var userProfileVM = fixture.Create<UserProfileVM>();

            // Act
            var action = await sut.UpdateProfile(userProfileVM, null);
            var view = action as ViewResult;

            // Assert
            Assert.IsType<ViewResult>(action);
            Assert.Equal("Index", view.ViewName);
        }

        [Theory]
        [InlineData("123", "321", null)]
        [InlineData("123", "321", "Name")]
        [InlineData("123", "321", "AboutMe")]
        [InlineData("123", "321", "BirthDate")]
        public async Task UpdateProfile_WithoutValidUser_ShouldReturn_InvalidModelState_ViewResult(string currentUserId, string savedUserId, string updateType) {

            var fixture = FixtureExtensions.CreateFixture();
            AddCustomizations(fixture);

            //Arange
            var uService = fixture.Freeze<Mock<IUserService>>();
            uService.Setup(s => s.GetUserId()).Returns(currentUserId);

            var sut = fixture.CreateController<UserProfileController>();
            var userProfileVM = fixture.Build<UserProfileVM>()
                .With(p => p.UserId, savedUserId)
                .Create();

            Enum.TryParse<UserProfileUpdateType>(updateType, out var uType);

            // Act
            var action = await sut.UpdateProfile(userProfileVM, updateType);
            var view = action as ViewResult;

            // Assert
            Assert.IsType<ViewResult>(action);
            Assert.Equal("Index", view.ViewName);
            Assert.False(sut.ModelState.IsValid);
            if (uType != UserProfileUpdateType.Unknown)
                Assert.Contains("UserId", sut.ModelState.Keys);
            else
                Assert.Contains("updateType", sut.ModelState.Keys);
        }

        [Theory]
        [InlineData("123", "123", null)]
        [InlineData("123", "123", "Name")]
        [InlineData("123", "123", "AboutMe")]
        [InlineData("123", "123", "BirthDate")]
        public async Task UpdateProfile_WithValidUser_ShouldReturn_RedirectToIndex(string currentUserId, string savedUserId, string updateType) {

            var fixture = FixtureExtensions.CreateFixture();
            AddCustomizations(fixture);

            //Arange
            var uService = fixture.Freeze<Mock<IUserService>>();
            uService.Setup(s => s.GetUserId()).Returns(currentUserId);

            var sut = fixture.CreateController<UserProfileController>();
            var userProfileVM = fixture.Build<UserProfileVM>()
                .With(p => p.UserId, savedUserId)
                .Create();

            Enum.TryParse<UserProfileUpdateType>(updateType, out var uType);

            // Act
            var action = await sut.UpdateProfile(userProfileVM, updateType);
            var redirect = action as RedirectToRouteResult;

            // Assert
            if (uType != UserProfileUpdateType.Unknown) {
                Assert.IsType<RedirectToRouteResult>(action);
                Assert.Contains("action", redirect.RouteValues.Keys);
                Assert.Equal("Index", redirect.RouteValues["action"]);
            } else
                Assert.Contains("updateType", sut.ModelState.Keys);
        }

        [Theory]
        [InlineData("Name")]
        [InlineData("AboutMe")]
        [InlineData("BirthDate")]
        public async Task UpdateProfile_ShouldCall_UserProfileRepo_InsertOrUpdate_ForValidUserId(string updateType) {

            var fixture = FixtureExtensions.CreateFixture();
            AddCustomizations(fixture);

            //Arange
            var userId = fixture.Create<string>();

            var uService = fixture.Freeze<Mock<IUserService>>();
            uService.Setup(u => u.GetUserId()).Returns(userId);

            var existingProfile = fixture.Build<UserProfile>()
                .With(p => p.UserId, userId)
                .Create();
            UserProfile userProfile;

            var upRepo = fixture.Freeze<Mock<IUserProfileRepository>>();
            upRepo.Setup(r => r.GetUserProfileAsync(It.IsAny<string>())).Returns(Task.FromResult(existingProfile));
            upRepo.Setup(r => r.InsertOrUpdate(It.IsAny<UserProfile>())).Callback<UserProfile>(profile => userProfile = profile);

            var sut = fixture.CreateController<UserProfileController>();
            var userProfileVM = fixture.Build<UserProfileVM>()
                .With(p => p.UserId, userId)
                .Create();

            // Act
            var view = await sut.UpdateProfile(userProfileVM, updateType);

            // Assert
            upRepo.Verify(r => r.InsertOrUpdate(It.Is<UserProfile>(_profile => _profile.UserId == userId)));
        }

        [Theory]
        [InlineData("Name")]
        [InlineData("AboutMe")]
        [InlineData("BirthDate")]
        public async Task UpdateProfile_ShouldNot_UpdateProfileForAnotherUser(string updateType) {

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
            var action = await sut.UpdateProfile(userProfileVM, updateType);
            var view = action as ViewResult;

            // Assert
            Assert.False(sut.ModelState.IsValid);
            Assert.True(sut.ModelState.ContainsKey("UserId"));
            Assert.Equal(userProfileVM, view.Model);
        }

        [Theory]
        [InlineData("Name")]
        [InlineData("AboutMe")]
        [InlineData("BirthDate")]
        public async Task UpdateProfile_TypeName_ShouldSave_TheUpdatedVM(string updateType) {

            var fixture = FixtureExtensions.CreateFixture();
            AddCustomizations(fixture);

            //Arange
            var loggedInUserId = fixture.Create<string>();

            var uService = fixture.Freeze<Mock<IUserService>>();
            uService.Setup(u => u.GetUserId()).Returns(loggedInUserId);

            var repositoryProfile = fixture.Build<UserProfile>()
                .With(p => p.UserId, loggedInUserId)
                .Create();


            var originalProfile = new UserProfile();

            mapper.Map(repositoryProfile, originalProfile, opts => opts.ConfigureMap());

            var upUOF = fixture.Freeze<Mock<IUserProfileUnitOfWork>>();
            upUOF.Setup(uof => uof.UserProfiles.GetUserProfileAsync(It.IsAny<string>()))
                .Returns(() => Task.FromResult(repositoryProfile));

            var sut = fixture.CreateController<UserProfileController>();
            var expectedVM = fixture.Build<UserProfileVM>()
                .With(profile => profile.UserId, loggedInUserId)
                .Create();

            // Act
            var action = await sut.UpdateProfile(expectedVM, updateType);

            // Assert
            upUOF.Verify(r => r.UserProfiles.GetUserProfileAsync(It.IsAny<string>()));
            upUOF.Verify(r => r.CompleteAsync());

            AssertUpdatedVMBasedOnUpdateType(originalProfile, expectedVM, repositoryProfile, updateType);
        }

        private static void AssertUpdatedVMBasedOnUpdateType(UserProfile originalProfile, UserProfileVM expectedVM, UserProfile model, string updateType) {
            Enum.TryParse(updateType, out UserProfileUpdateType uType);

            Assert.NotEqual(UserProfileUpdateType.Unknown, uType);

            Assert.Equal(originalProfile.Id, model.Id);
            Assert.Equal(originalProfile.UserId, model.UserId);
            Assert.Equal(originalProfile.User.UserName, model.User.UserName);
            Assert.Equal(originalProfile.User.Email, model.User.Email);

            switch (uType) {
                case UserProfileUpdateType.Name:
                    Assert.Equal(originalProfile.AboutMe, model.AboutMe);
                    Assert.Equal(originalProfile.BirthDate, model.BirthDate);

                    Assert.Equal(expectedVM.FirstName, model.FirstName);
                    Assert.Equal(expectedVM.LastName, model.LastName);
                    break;

                case UserProfileUpdateType.AboutMe:
                    Assert.Equal(expectedVM.AboutMe, model.AboutMe);

                    Assert.Equal(originalProfile.BirthDate, model.BirthDate);
                    Assert.Equal(originalProfile.FirstName, model.FirstName);
                    Assert.Equal(originalProfile.LastName, model.LastName);
                    break;

                case UserProfileUpdateType.BirthDate:
                    Assert.Equal(originalProfile.AboutMe, model.AboutMe);

                    Assert.Equal(expectedVM.BirthDate, model.BirthDate);

                    Assert.Equal(originalProfile.FirstName, model.FirstName);
                    Assert.Equal(originalProfile.LastName, model.LastName);
                    break;
            }
        }


        [Theory]
        [InlineData("AddGoal")]
        [InlineData("AddInterest")]
        [InlineData("AddStep")]
        public void AddEditDeleteMethod_ShouldBeAccessible_ToLoggedUsersOnly(string methodName) {

            var controllerType = typeof(UserProfileController);
            var dashboardMethodType = controllerType.GetMethod(methodName);
            Assert.NotNull(dashboardMethodType);

            var authAttrib = dashboardMethodType.CustomAttributes.FirstOrDefault(i => i.AttributeType == typeof(AuthorizeAttribute));
            Assert.NotNull(authAttrib);
        }

        [Fact]
        public async Task AddInterest_ShouldRetrieve_CurrentUserProfile() {
            var fixture = FixtureExtensions.CreateFixture();
            fixture.Customizations.Add(new ManyNavigationPropertyOmitter<UserProfile>());
            fixture.Customizations.Add(new ManyNavigationPropertyOmitter<Group>());

            // Arrange
            var currentUserId = fixture.Create<string>();
            var currentUserProfile = fixture.Build<UserProfile>()
                .With(p => p.Interests, new List<UserInterest>())
                .Create();

            var group = fixture.Build<Group>()
                .With(g => g.Members, new List<GroupMember>())
                .CreateMany(1)
                .ToList() as IList<Group>;

            var uService = fixture.Freeze<Mock<IUserService>>();
            uService.Setup(s => s.GetUserId()).Returns(currentUserId);

            var upRepo = fixture.Freeze<Mock<IUserProfileRepository>>();
            upRepo.Setup(r => r.GetUserProfileAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(currentUserProfile));

            var storyUOW = fixture.Freeze<Mock<IStoryUnitOfWork>>();
            storyUOW.Setup(uow => uow.Groups.GetAsync(It.IsAny<Expression<Func<Group, bool>>[]>()))
                .Returns(Task.FromResult(group));

            var interestId = fixture.Create<int>();
            var sut = fixture.CreateController<UserProfileController>();

            // Act
            var view = await sut.AddInterest(interestId);

            // Assert
            upRepo.Verify(r => r.GetUserProfileAsync(It.Is<string>(i => i == currentUserId)));

        }

        [Fact]
        public async Task AddInterest_ShouldNot_AddInvalidInterests() {
            var fixture = FixtureExtensions.CreateFixture();
            fixture.Customizations.Add(new ManyNavigationPropertyOmitter<UserProfile>());

            // Arrange
            var interestId = fixture.Create<int>();
            var interest = fixture.Build<Interest>()
                .With(i => i.Id, interestId)
                .Create();

            var interestRepo = fixture.Freeze<Mock<IInterestRepository>>();
            interestRepo.Setup(r => r.GetAsync(It.IsAny<Expression<Func<Interest, bool>>[]>())).Returns(Task.FromResult(new List<Interest>() as IList<Interest>));

            var sut = fixture.CreateController<UserProfileController>();

            // Act
            var view = await sut.AddInterest(interestId);

            // Assert
            Assert.False(sut.ModelState.IsValid);
            Assert.Contains("Interest", sut.ModelState.Keys);

            // Check that the expression used to match interest is evaluating properly
            interestRepo.Verify(r => r.GetAsync(
                It.Is<Expression<Func<Interest, bool>>[]>(exprs => 
                    exprs.Select(e => e.Compile()(interest)).All(x => x)
                )
            ));
        }

        [Fact]
        public async Task AddInterest_ShouldInsertAndSaveToDatabase() {
            var fixture = FixtureExtensions.CreateFixture();
            fixture.Customizations.Add(new ManyNavigationPropertyOmitter<UserProfile>());
            fixture.Customizations.Add(new ManyNavigationPropertyOmitter<Group>());

            // Arrange
            var currentUserId = fixture.Create<string>();
            var currentUserProfileId = fixture.Create<int>();

            var interestId = fixture.Create<int>();

            var currentUserInterests = fixture.CreateMany<UserInterest>()
                .Where(i => i.InterestId != interestId)
                .ToList();

            var uService = fixture.Freeze<Mock<IUserService>>();
            uService.Setup(s => s.GetUserId()).Returns(currentUserId);

            var currentUserProfile = fixture.Build<UserProfile>()
                .With(p => p.Id, currentUserProfileId)
                .With(p => p.Interests, currentUserInterests)
                .Create();

            var group = fixture.Build<Group>()
                .With(g => g.Members, new List<GroupMember>())
                .CreateMany(1)
                .ToList() as IList<Group>;

            var upUOW = fixture.Freeze<Mock<IUserProfileUnitOfWork>>();
            upUOW.Setup(uow => uow.UserProfiles.GetUserProfileAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(currentUserProfile));

            var storyUOW = fixture.Freeze<Mock<IStoryUnitOfWork>>();
            storyUOW.Setup(uow => uow.Groups.GetAsync(It.IsAny<Expression<Func<Group, bool>>[]>()))
                .Returns(Task.FromResult(group));

            var sut = fixture.CreateController<UserProfileController>();

            // Act
            var action = await sut.AddInterest(interestId);
            var redirect = action as RedirectToRouteResult;
            // Assert
            upUOW.Verify(uow => uow.UserProfiles.InsertOrUpdateGraph(
                It.Is<UserProfile>(p =>
                    p.Interests.Where(i =>
                        i.UserProfileId == currentUserProfileId && i.InterestId == interestId).Count() == 1
                    )
                )
            );
            upUOW.Verify(uow => uow.CompleteAsync());

            Assert.IsType<RedirectToRouteResult>(action);
            Assert.Contains("action", redirect.RouteValues.Keys);
            Assert.Equal("Index", redirect.RouteValues["action"]);
        }
    }
}
