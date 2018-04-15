using AutoFixture;
using Moq;
using Project.Account.Models;
using Project.Core.Account;
using Project.Core.DbContext;
using Project.Tests.Utils;
using Project.UserProfileDomain.DAL;
using Project.UserProfileDomain.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Project.Tests.Repositories.UserProfile {
    public class UserProfileRepositoryTests {

        [Fact]
        public async Task All_ShouldReturn_AllEntitiesInContext() {
            var fixture = FixtureExtensions.CreateFixture();
            fixture.Customizations.Add(new ManyNavigationPropertyOmitter<UserProfileDomain.Models.UserProfile>());

            // Arrange
            var storeSize = fixture.Create<int>();
            var profileStore = fixture.CreateMany<UserProfileDomain.Models.UserProfile>(storeSize);

            var upContext = fixture.FreezeDbContext<IUserProfileContext>();
            MockingHelpers.MockDbContextSet(upContext, c => c.UserProfiles, profileStore.AsQueryable());

            var sut = fixture.Create<UserProfileRepository>();

            // Act
            var actual = sut.All;
            var actualAsync = await sut.AllAsync;

            //Assert
            Assert.Equal(profileStore, actual);
            Assert.Equal(profileStore, actualAsync);
        }

        [Fact]
        public async Task Get_ShouldReturn_FilteredItems() {
            var fixture = FixtureExtensions.CreateFixture();
            fixture.Customizations.Add(new ManyNavigationPropertyOmitter<UserProfileDomain.Models.UserProfile>());

            // Arrange
            var testBirthDate = fixture.Create<DateTime>();
            var nonMatchingCount = fixture.Create<int>();
            var mathcingCount = fixture.Create<int>();

            var matching = fixture.Build<UserProfileDomain.Models.UserProfile>()
                .With(p => p.BirthDate, testBirthDate)
                .CreateMany(mathcingCount);
            var profileStore = matching.Concat(fixture.CreateMany<UserProfileDomain.Models.UserProfile>(nonMatchingCount)).ToList();

            var upContext = fixture.FreezeDbContext<IUserProfileContext>();
            MockingHelpers.MockDbContextSet(upContext, c => c.UserProfiles, profileStore.AsQueryable());

            var sut = fixture.Create<UserProfileRepository>();

            // Act
            var actual = sut.Get(p => p.BirthDate == testBirthDate);
            var actualAsync = await sut.GetAsync(p => p.BirthDate == testBirthDate);

            //Assert
            Assert.Equal(matching, actual);
            Assert.Equal(matching, actualAsync);
        }

        [Fact]
        public async Task GetUserProfile_ShouldReturn_UserProfile() {
            var fixture = FixtureExtensions.CreateFixture();
            fixture.Customizations.Add(new ManyNavigationPropertyOmitter<UserProfileDomain.Models.UserProfile>());

            // Arrange
            var testUserId = fixture.Create<string>();
            var count = fixture.Create<int>();

            var expectedProfile = fixture.Build<UserProfileDomain.Models.UserProfile>()
                .With(p => p.UserId, testUserId)
                .Create();
            var profileStore = new List<UserProfileDomain.Models.UserProfile>() { expectedProfile }
                .Concat(fixture.Build<UserProfileDomain.Models.UserProfile>()
                    .With(p => p.UserId, fixture.Create<string>())
                    .CreateMany(count));

            var upContext = fixture.FreezeDbContext<IUserProfileContext>();
            MockingHelpers.MockDbContextSet(upContext, c => c.UserProfiles, profileStore.AsQueryable());

            var sut = fixture.Create<UserProfileRepository>();

            // Act
            var actual = sut.GetUserProfile(testUserId);
            var actualAsync = await sut.GetUserProfileAsync(testUserId);

            //Assert
            Assert.Equal(expectedProfile, actual);
            Assert.Equal(expectedProfile, actualAsync);
        }

        [Fact]
        public async Task GetStrictInRoleUserProfilesAsync() {
            var fixture = FixtureExtensions.CreateFixture();
            fixture.Customizations.Add(new ManyNavigationPropertyOmitter<UserProfileDomain.Models.UserProfile>());

            // Arrange
            var testRole = fixture.Create<StandardRoles>();
            var matchCount = fixture.Create<int>();
            var count = fixture.Create<int>();

            var expectedProfiles = CreateUsers(fixture, matchCount, () =>
                fixture.CreateMany<StandardRoles>()
                    .Where(r => r < testRole)
                    .Concat(new[] { testRole })
                    .Distinct()
                    .ToList()
            ).ToList();

            var profileStore = expectedProfiles.Concat(
                CreateUsers(fixture, count, () =>
                    fixture.CreateMany<StandardRoles>()
                        .Where(r => r != testRole)
                        .Distinct()
                        .ToList()
                )
            ).ToList();

            var upContext = fixture.FreezeDbContext<IUserProfileContext>();
            MockingHelpers.MockDbContextSet(upContext, c => c.UserProfiles, profileStore.AsQueryable());

            var sut = fixture.Create<UserProfileRepository>();

            // Act
            var actualAsync = await sut.GetStrictInRoleUserProfilesAsync(testRole);

            //Assert
            Assert.Equal(expectedProfiles, actualAsync);
        }

        [Fact]
        public async Task GetUsersInRoleProfileAsync() {
            var fixture = FixtureExtensions.CreateFixture();
            fixture.Customizations.Add(new ManyNavigationPropertyOmitter<UserProfileDomain.Models.UserProfile>());

            // Arrange
            var testRole = fixture.Create<StandardRoles>();
            var matchCount = fixture.Create<int>();
            var count = fixture.Create<int>();

            var expectedProfiles = CreateUsers(fixture, matchCount, () =>
                fixture.CreateMany<StandardRoles>()
                    .Concat(new[] { testRole })
                    .Distinct()
                    .ToList()
            ).ToList();

            var profileStore = expectedProfiles.Concat(
                CreateUsers(fixture, count, () =>
                    fixture.CreateMany<StandardRoles>()
                        .Where(r => r != testRole)
                        .Distinct()
                        .ToList()
                )
            ).ToList();

            var upContext = fixture.FreezeDbContext<IUserProfileContext>();
            MockingHelpers.MockDbContextSet(upContext, c => c.UserProfiles, profileStore.AsQueryable());

            var sut = fixture.Create<UserProfileRepository>();

            // Act
            var actualAsync = await sut.GetUsersInRoleProfileAsync(testRole);

            //Assert
            Assert.Equal(expectedProfiles, actualAsync);
            if (expectedProfiles.Any(p => p.User.Roles.Count > 1))
                Assert.Contains(actualAsync, p => p.User.Roles.Count > 1);
        }

        private IEnumerable<UserProfileDomain.Models.UserProfile> CreateUsers(IFixture fixture, int count, Func<IList<StandardRoles>> roles) {
            return fixture.Build<UserProfileDomain.Models.UserProfile>()
                .Without(p => p.User)
                .Do(p => p.User = CreateUser(fixture, roles()))
                .CreateMany(count);
        }

        private UserInfo CreateUser(IFixture fixture, IList<StandardRoles> roles) {
            return fixture.Build<UserInfo>()
                 .Do(u => {
                     foreach (var role in roles)
                         u.Roles.Add(CreateRole(fixture, role));
                 })
                 .Create();
        }

        private UserRoleInfo CreateRole(IFixture fixture, StandardRoles role) {
            return fixture.Build<UserRoleInfo>()
                .With(ur => ur.Role, fixture.Build<RoleInfo>()
                    .With(r => r.Name, role.ToString())
                    .Create())
                .Create();
        }
    }
}
