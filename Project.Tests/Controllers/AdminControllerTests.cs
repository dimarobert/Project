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

namespace Project.Tests.Controllers {
    public class AdminControllerTests {

        public AdminControllerTests() {
            AutoMapperUtil.ConfigureOnce();
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
            fixture.Customizations.Add(new ManyNavigationPropertyOmitter<UserProfile>());

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
            fixture.Customizations.Add(new ManyNavigationPropertyOmitter<UserProfile>());

            // Arrange

            var uService = fixture.Freeze<Mock<IUserService>>();
            uService.Setup(s => s.IsInRole(It.IsAny<StandardRoles>())).Returns(true);
            uService.Setup(s => s.IsInRole(It.IsAny<string>())).Returns(true);

            var upRepo = fixture.Freeze<Mock<IUserProfileRepository>>();
            var interestRepo = fixture.Freeze<Mock<IInterestRepository>>();

            var sut = fixture.CreateController<AdminController>();

            // Act
            var action = await sut.Dashboard();

            // Assert
            upRepo.Verify(r => r.GetUsersInRoleProfileAsync(It.Is<StandardRoles>(sr => sr == StandardRoles.Normal)));
            upRepo.Verify(r => r.GetStrictInRoleUserProfilesAsync(It.Is<StandardRoles>(sr => sr == StandardRoles.Coach)));
            interestRepo.Verify(r => r.AllAsync);

        }

        [Fact]
        public async Task Dasboard_ShouldReturn_MappedValuesFromRepository() {
            var fixture = FixtureExtensions.CreateFixture();
            fixture.Customizations.Add(new ManyNavigationPropertyOmitter<UserProfile>());

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


            var upRepo = fixture.Freeze<Mock<IUserProfileRepository>>();
            upRepo.Setup(r => r.GetUsersInRoleProfileAsync(It.Is<StandardRoles>(sr => sr == StandardRoles.Normal)))
                .Returns(Task.FromResult(expectedRegularUsers as IList<UserProfile>));
            upRepo.Setup(r => r.GetStrictInRoleUserProfilesAsync(It.Is<StandardRoles>(sr => sr == StandardRoles.Coach)))
                .Returns(Task.FromResult(expectedCoachUsers as IList<UserProfile>));

            var interestsRepo = fixture.Freeze<Mock<IInterestRepository>>();
            interestsRepo.Setup(r => r.AllAsync)
                .Returns(Task.FromResult(expectedInterests as IList<Interest>));

            var sut = fixture.CreateController<AdminController>();

            // Act
            var action = await sut.Dashboard() as ViewResult;
            var model = action.Model as DashboardVM;
            // Assert

            Assert.Equal(expectedRegularUsers.Count, model.RegularUsers.Count);
            Assert.Equal(expectedCoachUsers.Count, model.Coaches.Count);
            Assert.Equal(expectedInterests.Count, model.Interests.Count);
            AssertPocoListEqualsVmList(expectedRegularUsers, model.RegularUsers);
            AssertPocoListEqualsVmList(expectedCoachUsers, model.Coaches);
            AssertPocoListEqualsVmList(expectedInterests, model.Interests);
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

    }
}
