using AutoFixture;
using Moq;
using Project.Services.Account;
using Project.Tests.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using Xunit;

namespace Project.Tests.Services.Account {
    public class UserServiceTests {

        [Fact]
        public void IsAuthenticated_ReturnsFalse_GetUserId_ReturnsNull_WithNullUser() {
            var fixture = FixtureExtensions.CreateFixture();

            // Arrange
            fixture.Register<IPrincipal>(() => null);

            var sut = fixture.Create<UserService>();

            // Act
            bool isAuthenticated = sut.IsAuthenticated;
            string userId = sut.GetUserId();

            // Assert
            Assert.False(isAuthenticated, "IsAuthenticated must be false for null IPrincipal.");
            Assert.Null(userId);
        }

        [Fact]
        public void IsAuthenticated_ReturnsFalse_GetUserId_ReturnsNull_WithNullIdentity() {
            var fixture = FixtureExtensions.CreateFixture();

            // Arrange
            var usr = fixture.Freeze<Mock<IPrincipal>>();
            usr.Setup(i => i.Identity).Returns((IIdentity)null);

            var sut = fixture.Create<UserService>();

            // Act
            bool isAuthenticated = sut.IsAuthenticated;
            string userId = sut.GetUserId();

            // Assert
            Assert.False(isAuthenticated, "IsAuthenticated must be false for null IIdentity.");
            Assert.Null(userId);
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


    }
}
