using AutoFixture;
using Moq;
using Project.Account.Models;
using Project.Account.Services;
using Project.Controllers;
using Project.Core.Account;
using Project.Tests.Utils;
using Project.UserProfileDomain.Models;
using Project.UserProfileDomain.Repositories;
using Project.ViewModels.UserProfile;
using Project.ViewModels.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xunit;
using Project.StoryDomain.Models;
using Project.StoryDomain.Repositories;
using Project.ViewModels.Account;

namespace Project.Tests.Controllers {
    public class AdminControllerTests {

        public AdminControllerTests() {
            AutoMapperUtil.ConfigureOnce();
        }

        private static void AddFixtureCustomizations(IFixture fixture) {
            fixture.Customizations.Add(new ManyNavigationPropertyOmitter<UserProfile>());
            fixture.Customizations.Add(new ManyNavigationPropertyOmitter<Group>());
        }

        [Fact]
        public void Index_ShouldRedirectTo_Dashboard() {
            var fixture = FixtureExtensions.CreateFixture();

            var sut = fixture.CreateController<AdminController>();

            // Act
            var action = sut.Index();
            var redirect = action as RedirectToRouteResult;

            // Assert
            Assert.IsType<RedirectToRouteResult>(action);
            Assert.Contains("action", redirect.RouteValues.Keys);
            Assert.Equal("Dashboard", redirect.RouteValues["action"]);
        }

        [Fact]
        public void Dashboard_ShouldBeVisible_ToAdminsOnly() {

            var controllerType = typeof(AdminController);
            var dashboardMethodType = controllerType.GetMethod("Dashboard");
            Assert.NotNull(dashboardMethodType);

            var authAttrib = dashboardMethodType.CustomAttributes.FirstOrDefault(i => i.AttributeType == typeof(AuthorizeAttribute));
            Assert.NotNull(authAttrib);

            var rolesParam = authAttrib.NamedArguments.FirstOrDefault(a => a.MemberName == "Roles");
            Assert.NotEqual(default(CustomAttributeNamedArgument), rolesParam);

            Assert.Equal("Admin", rolesParam.TypedValue.Value);
        }

        [Fact]
        public async Task Dasboard_ShouldReturn_DashboardVM() {
            var fixture = FixtureExtensions.CreateFixture();
            AddFixtureCustomizations(fixture);

            // Arrange

            var uService = fixture.Freeze<Mock<IUserService>>();
            uService.Setup(s => s.IsInRole(It.IsAny<StandardRoles>())).Returns(true);
            uService.Setup(s => s.IsInRole(It.IsAny<string>())).Returns(true);

            var sut = fixture.CreateController<AdminController>();

            // Act
            var action = await sut.Dashboard();
            var view = action as ViewResult;

            // Assert
            Assert.IsType<ViewResult>(action);
            Assert.IsType<DashboardVM>(view.Model);
        }

       

        [Fact]
        public async Task Dasboard_ShouldQueryRepositories() {
            var fixture = FixtureExtensions.CreateFixture();
            AddFixtureCustomizations(fixture);

            // Arrange

            var uService = fixture.Freeze<Mock<IUserService>>();
            uService.Setup(s => s.IsInRole(It.IsAny<StandardRoles>())).Returns(true);
            uService.Setup(s => s.IsInRole(It.IsAny<string>())).Returns(true);

            var upRepo = fixture.Freeze<Mock<IUserProfileRepository>>();
            var interestRepo = fixture.Freeze<Mock<IInterestRepository>>();
            var hashtagRepo = fixture.Freeze<Mock<IHashtagRepository>>();
            var groupRepo = fixture.Freeze<Mock<IGroupRepository>>();

            var sut = fixture.CreateController<AdminController>();

            // Act
            var action = await sut.Dashboard();

            // Assert
            upRepo.Verify(r => r.GetUsersInRoleProfileAsync(It.Is<StandardRoles>(sr => sr == StandardRoles.Normal)));
            upRepo.Verify(r => r.GetStrictInRoleUserProfilesAsync(It.Is<StandardRoles>(sr => sr == StandardRoles.Coach)));
            interestRepo.Verify(r => r.AllAsync);
            hashtagRepo.Verify(r => r.AllAsync);
            groupRepo.Verify(r => r.AllAsync);

        }

        [Fact]
        public async Task Dasboard_ShouldReturn_MappedValuesFromRepository() {
            var fixture = FixtureExtensions.CreateFixture();
            AddFixtureCustomizations(fixture);

            // Arrange

            var uService = fixture.Freeze<Mock<IUserService>>();
            uService.Setup(s => s.IsInRole(It.IsAny<StandardRoles>())).Returns(true);
            uService.Setup(s => s.IsInRole(It.IsAny<string>())).Returns(true);

            var expectedRegularUsers = fixture.Build<UserProfile>()
                .CreateMany()
                .ToList();
            var expectedCoachUsers = fixture.Build<UserProfile>()
                .CreateMany()
                .ToList();

            var expectedInterests = fixture.Build<Interest>()
                .CreateMany()
                .ToList();

            var expectedGroups = fixture.Build<Group>()
                .With(g => g.Members, fixture.CreateMany<GroupMember>().ToList())
                .CreateMany()
                .ToList();

            var expectedHashtags = fixture.Build<Hashtag>()
               .CreateMany()
               .ToList();


            var unitOfWork = fixture.Freeze<Mock<IUserProfileUnitOfWork>>();
            unitOfWork.Setup(uof => uof.UserProfiles.GetUsersInRoleProfileAsync(It.Is<StandardRoles>(sr => sr == StandardRoles.Normal)))
                .Returns(Task.FromResult(expectedRegularUsers as IList<UserProfile>));
            unitOfWork.Setup(uof => uof.UserProfiles.GetStrictInRoleUserProfilesAsync(It.Is<StandardRoles>(sr => sr == StandardRoles.Coach)))
                .Returns(Task.FromResult(expectedCoachUsers as IList<UserProfile>));

            unitOfWork.Setup(uof => uof.Interests.AllAsync)
                .Returns(Task.FromResult(expectedInterests as IList<Interest>));

            var storyUOF = fixture.Freeze<Mock<IStoryUnitOfWork>>();
            storyUOF.Setup(uof => uof.Groups.AllAsync)
                .Returns(Task.FromResult(expectedGroups as IList<Group>));
            storyUOF.Setup(uof => uof.Hashtags.AllAsync)
                .Returns(Task.FromResult(expectedHashtags as IList<Hashtag>));

            var sut = fixture.CreateController<AdminController>();

            // Act
            var action = await sut.Dashboard() as ViewResult;
            var model = action.Model as DashboardVM;
            // Assert

            Assert.Equal(expectedRegularUsers.Count, model.RegularUsers.Count);
            Assert.Equal(expectedCoachUsers.Count, model.Coaches.Count);
            Assert.Equal(expectedInterests.Count, model.Interests.Count);
            Assert.Equal(expectedGroups.Count, model.Groups.Count);
            Assert.Equal(expectedHashtags.Count, model.Hashtags.Count);
            AssertPocoListEqualsVmList(expectedRegularUsers, model.RegularUsers);
            AssertPocoListEqualsVmList(expectedCoachUsers, model.Coaches);
            AssertPocoListEqualsVmList(expectedInterests, model.Interests);
            AssertPocoListEqualsVmList(expectedGroups, model.Groups);
        }

        [Fact]
        public async Task GrantRole_ShouldReturnError_ForInvalidUser() {
            var fixture = FixtureExtensions.CreateFixture();

            var grantRoleVM = fixture.Create<GrantRoleVM>();

            var uService = fixture.Freeze<Mock<IUserService>>();
            uService.Setup(s => s.FindUserByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<UserInfo>(null));

            var sut = fixture.CreateController<AdminController>();

            // Act
            var action = await sut.GrantRole(grantRoleVM);
            var pView = action as PartialViewResult;
            // Assert
            Assert.IsType<PartialViewResult>(action);
            Assert.Equal("_AjaxValidation", pView.ViewName);

            Assert.False(sut.ModelState.IsValid);
            Assert.Contains("UserId", sut.ModelState.Keys);

        }

        [Fact]
        public async Task GrantRole_ShouldReturnError_IfUserAlreadyInRole() {
            var fixture = FixtureExtensions.CreateFixture();

            var grantRoleVM = fixture.Create<GrantRoleVM>();

            var uService = fixture.Freeze<Mock<IUserService>>();
            uService.Setup(s => s.FindUserByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new UserInfo()));

            uService.Setup(s => s.IsInRole(It.IsAny<string>(), It.IsAny<string>()))
               .Returns(true);

            var sut = fixture.CreateController<AdminController>();

            // Act
            var action = await sut.GrantRole(grantRoleVM);
            var pView = action as PartialViewResult;
            
            // Assert
            uService.Verify(s => s.IsInRole(It.Is<string>(v => v == grantRoleVM.UserId), It.Is<string>(v => v == grantRoleVM.RoleName)));

            Assert.IsType<PartialViewResult>(action);
            Assert.Equal("_AjaxValidation", pView.ViewName);

            Assert.False(sut.ModelState.IsValid);
            Assert.Contains("UserAlreadyInRole", sut.ModelState.Keys);

        }


        [Fact]
        public async Task GrantRole_ShouldReturnError_IfFailsToAddUserToRole() {
            var fixture = FixtureExtensions.CreateFixture();

            var grantRoleVM = fixture.Create<GrantRoleVM>();
            var errorMsgs = fixture.CreateMany<string>().ToList();

            var uService = fixture.Freeze<Mock<IUserService>>();
            uService.Setup(s => s.FindUserByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new UserInfo()));

            uService.Setup(s => s.IsInRole(It.IsAny<string>(), It.IsAny<string>()))
               .Returns(false);

            uService.Setup(s => s.AddToRoleAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(GrantRoleResult.Failed(errorMsgs)));

            var sut = fixture.CreateController<AdminController>();

            // Act
            var action = await sut.GrantRole(grantRoleVM);
            var pView = action as PartialViewResult;

            // Assert
            uService.Verify(s => s.IsInRole(It.Is<string>(v => v == grantRoleVM.UserId), It.Is<string>(v => v == grantRoleVM.RoleName)));

            Assert.IsType<PartialViewResult>(action);
            Assert.Equal("_AjaxValidation", pView.ViewName);

            Assert.False(sut.ModelState.IsValid);
            Assert.Contains("GrantRoleAction", sut.ModelState.Keys);
            ModelState actualErrMsgs = sut.ModelState["GrantRoleAction"];
            Assert.Equal(errorMsgs.Count, actualErrMsgs.Errors.Count);
            Assert.Equal(errorMsgs, actualErrMsgs.Errors.Select(e => e.ErrorMessage));

        }

        [Fact]
        public async Task GrantRole_ShouldReturn_JsonResponse_OnSuccess() {
            var fixture = FixtureExtensions.CreateFixture();

            var grantRoleVM = fixture.Create<GrantRoleVM>();
            var urlLocation = fixture.Create<string>();

            var uService = fixture.Freeze<Mock<IUserService>>();
            uService.Setup(s => s.FindUserByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new UserInfo()));

            uService.Setup(s => s.IsInRole(It.IsAny<string>(), It.IsAny<string>()))
               .Returns(false);

            uService.Setup(s => s.AddToRoleAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(GrantRoleResult.Success));

            var mockUrlHelper = new Mock<UrlHelper>();
            mockUrlHelper.Setup(m => m.Action(It.IsAny<string>()))
                .Returns(urlLocation);

            var sut = fixture.CreateController<AdminController>();
            sut.Url = mockUrlHelper.Object;

            // Act
            var action = await sut.GrantRole(grantRoleVM);
            var jsonResult = action as JsonResult;

            // Assert
            uService.Verify(s => s.IsInRole(It.Is<string>(v => v == grantRoleVM.UserId), It.Is<string>(v => v == grantRoleVM.RoleName)));

            Assert.IsType<JsonResult>(action);

            var jsonType = jsonResult.Data.GetType();
            Assert.Contains("location", jsonType.GetProperties().Select(m => m.Name));
            Assert.Equal(urlLocation, jsonType.GetProperty("location").GetValue(jsonResult.Data));

            Assert.True(sut.ModelState.IsValid);
        }

        private void AssertPocoListEqualsVmList(IList<UserProfile> expected, IList<UserBasicInfoVM> actual) {
            for (int i = 0; i < expected.Count; i++) {
                AssertPocoEqualsVM(expected[i], actual[i]);
            }
        }

        private void AssertPocoEqualsVM(UserProfile expected, UserBasicInfoVM actual) {
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.FirstName, actual.FirstName);
            Assert.Equal(expected.LastName, actual.LastName);
            Assert.Equal(expected.BirthDate, actual.BirthDate);
            Assert.Equal(expected.User.Email, actual.Email);
            Assert.Equal(expected.User.UserName, actual.UserName);
            Assert.Equal(expected.UserId, actual.UserId);
        }

        private void AssertPocoListEqualsVmList(IList<Interest> expected, IList<InterestVM> actual) {
            for (int i = 0; i < expected.Count; i++) {
                AssertPocoEqualsVM(expected[i], actual[i]);
            }
        }

        private void AssertPocoEqualsVM(Interest expected, InterestVM actual) {
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Title, actual.Title);
        }

        private void AssertPocoListEqualsVmList(IList<Group> expected, IList<GroupVM> actual) {
            for (int i = 0; i < expected.Count; i++) {
                AssertPocoEqualsVM(expected[i], actual[i]);
            }
        }

        private void AssertPocoEqualsVM(Group expected, GroupVM actual) {
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Members.Count, actual.Members.Count);
        }
    }
}
