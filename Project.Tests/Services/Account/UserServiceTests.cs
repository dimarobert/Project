using AutoFixture;
using Moq;
using Project.Account.Managers;
using Project.Account.Models;
using Project.Account.Services;
using Project.Tests.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Project.Tests.Services.Account {
    public class UserServiceTests {

        [Fact]
        public void Constructor_ThrowsFor_NullUserManager() {
            var fixture = FixtureExtensions.CreateFixture();

            // Arrange
            fixture.Register<ApplicationUserManager>(() => null);

            // Assert
            Assert.Throws<ArgumentNullException>("userManager", () => new UserService(null, null));
        }

        [Fact]
        public void IsAuthenticated_ReturnsFalse_GetUserId_GetUserName_ReturnsNull_WithNullUser() {
            var fixture = FixtureExtensions.CreateFixture();

            // Arrange
            fixture.Register<IPrincipal>(() => null);

            var sut = fixture.Create<UserService>();

            // Act
            bool isAuthenticated = sut.IsAuthenticated;
            string userId = sut.GetUserId();
            string userName = sut.GetUserName();

            // Assert
            Assert.False(isAuthenticated, "IsAuthenticated must be false for null IPrincipal.");
            Assert.Null(userId);
            Assert.Null(userName);
        }

        [Fact]
        public void IsAuthenticated_ReturnsFalse_GetUserId_GetUserName_ReturnsNull_WithNullIdentity() {
            var fixture = FixtureExtensions.CreateFixture();

            // Arrange
            var usr = fixture.Freeze<Mock<IPrincipal>>();
            usr.Setup(i => i.Identity).Returns((IIdentity)null);

            var sut = fixture.Create<UserService>();

            // Act
            bool isAuthenticated = sut.IsAuthenticated;
            string userId = sut.GetUserId();
            string userName = sut.GetUserName();

            // Assert
            Assert.False(isAuthenticated, "IsAuthenticated must be false for null IIdentity.");
            Assert.Null(userId);
            Assert.Null(userName);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void IsAuthenticated_ReturnsValueOf_UserIdentityIsAuthenticated(bool isAuthVal) {
            var fixture = FixtureExtensions.CreateFixture();

            // Arrange
            var id = fixture.Freeze<Mock<IIdentity>>();
            id.Setup(i => i.IsAuthenticated).Returns(isAuthVal);

            var usr = fixture.Freeze<Mock<IPrincipal>>();
            usr.Setup(i => i.Identity).Returns(id.Object);

            var sut = fixture.Create<UserService>();

            // Act
            bool isAuthenticated = sut.IsAuthenticated;

            // Assert
            Assert.Equal(isAuthVal, isAuthenticated);
        }

        // GetUserId and GetUserName are Extension Methods and cannot be tested


        [Fact]
        public async Task FindUserByName_CallsUserManager() {
            var fixture = FixtureExtensions.CreateFixture();

            // Arrange
            var userInfo = fixture.Create<UserInfo>();

            var userMgr = fixture.Freeze<Mock<ApplicationUserManager>>();
            userMgr.Setup(mgr => mgr.FindByNameAsync(It.IsAny<string>())).Returns(Task.FromResult(userInfo));

            var sut = new UserService(userMgr.Object, null);

            // Act
            var actualUserInfo = await sut.FindUserByName(userInfo.UserName);

            // Assert
            userMgr.Verify(mgr => mgr.FindByNameAsync(It.Is<string>(_uName => _uName == userInfo.UserName)));
            Assert.Equal(userInfo, actualUserInfo);
        }
    }
}
