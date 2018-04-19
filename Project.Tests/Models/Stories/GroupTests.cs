using AutoFixture;
using AutoMapper;
using Project.StoryDomain.Models;
using Project.Tests.Utils;
using Project.ViewModels.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Project.Tests.Models.Stories {
    public class GroupTests {


        public GroupTests() {
            AutoMapperUtil.ConfigureOnce();
        }

        [Fact]
        public void GroupMember_MapsTo_UserProfileRefVM() {
            var fixture = FixtureExtensions.CreateFixture();
            fixture.Customizations.Add(new ManyNavigationPropertyOmitter<UserProfileDomain.Models.UserProfile>());
            fixture.Customizations.Add(new ManyNavigationPropertyOmitter<Group>());

            // Arrange
            var groupMembers = fixture.CreateMany<GroupMember>().ToList();

            // Act
            var uprVM = Mapper.Map<IList<UserProfileRefVM>>(groupMembers);

            // Assert
            Assert.Equal(groupMembers.Count, uprVM.Count);

            var joined = uprVM.Join(groupMembers, up => up.Id, gm => gm.UserProfile.Id, (up, gm) => new { up, gm })
                .ToList();

            Assert.Equal(groupMembers.Count, joined.Count);

            Assert.All(joined, j => {
                Assert.Equal(j.gm.UserProfile.FirstName, j.up.FirstName);
                Assert.Equal(j.gm.UserProfile.LastName, j.up.LastName);
            });
        }

        [Fact]
        public void Group_MapsTo_GroupVM() {
            var fixture = FixtureExtensions.CreateFixture();
            fixture.Customizations.Add(new ManyNavigationPropertyOmitter<UserProfileDomain.Models.UserProfile>());
            fixture.Customizations.Add(new ManyNavigationPropertyOmitter<Group>());

            // Arrange
            var groups = fixture.Build<Group>()
                .With(g => g.Members, fixture.CreateMany<GroupMember>().ToList())
                .CreateMany()
                .ToList();

            // Act
            var groupVM = Mapper.Map<IList<GroupVM>>(groups);

            // Assert
            Assert.Equal(groups.Count, groupVM.Count);

            var joinedGroups = groupVM.Join(groups, gvm => gvm.Id, g => g.Id, (gvm, g) => new { gvm, g }).ToList();

            Assert.Equal(groups.Count, joinedGroups.Count);

            Assert.All(joinedGroups, jGroup => {
                Assert.Equal(jGroup.g.Title, jGroup.gvm.Title);
                Assert.Equal(jGroup.g.Members.Count, jGroup.gvm.Members.Count);

                var joinedMembers = jGroup.gvm.Members.Join(jGroup.g.Members, up => up.Id, gm => gm.UserProfile.Id, (up, gm) => new { up, gm }).ToList();

                Assert.Equal(jGroup.g.Members.Count, joinedMembers.Count);

                Assert.All(joinedMembers, jMember => {
                    Assert.Equal(jMember.gm.UserProfile.FirstName, jMember.up.FirstName);
                    Assert.Equal(jMember.gm.UserProfile.LastName, jMember.up.LastName);
                });
            });
        }

    }


}
