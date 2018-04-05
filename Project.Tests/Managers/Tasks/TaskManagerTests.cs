using Project.Managers.Tasks;
using Project.Tests.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using AutoFixture;
using Moq;
using Project.DAL.Tasks;
using Project.Models.Tasks;

namespace Project.Tests.Managers.Tasks {
    public class TaskManagerTests {

        [Fact]
        public void ShouldThrow_ArgumentNullExc_WithNull_TaskDbCtx() {
            var fixture = FixtureExtensions.CreateFixture();

            // Arrange
            fixture.Register<ITaskDbContext>(() => null);

            // Assert
            Assert.Throws<ArgumentNullException>("taskDbContext", () => new TaskManager(null));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        public void Should_ReturnEmptyList_WithNullOrWhitespace_UserId(string userId) {
            var fixture = FixtureExtensions.CreateFixture();

            // Arrange
            RegisterTaskDbContext(fixture, null);

            var sut = fixture.Create<TaskManager>();

            // Act
            var userTasks = sut.GetUserTasks(userId);

            // Assert
            Assert.Equal(0, userTasks.Count);
        }

        [Fact]
        public void Should_ReturnEmptyList_WithNull_TasksList() {
            var fixture = FixtureExtensions.CreateFixture();

            // Arrange
            RegisterTaskDbContext(fixture, null);

            var sut = fixture.Create<TaskManager>();

            // Act
            var userTasks = sut.GetUserTasks("1");

            // Assert
            Assert.Equal(0, userTasks.Count);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        public void Should_ReturnAllValues_WithSameUserId(int numberOfTasks) {
            var fixture = FixtureExtensions.CreateFixture();

            var userId = "1";
            // Arrange
            var tasks = fixture
                .Build<Task>()
                .With(t => t.UserId, userId).CreateMany(numberOfTasks).AsQueryable();

            RegisterTaskDbContext(fixture, tasks);

            var sut = fixture.Create<TaskManager>();

            // Act
            var userTasks = sut.GetUserTasks(userId);

            // Assert
            Assert.NotNull(userTasks);
            Assert.Equal(numberOfTasks, userTasks.Count);
        }

        private void RegisterTaskDbContext(IFixture fixture, IQueryable<Task> tasks) {
            var taskDbSet = tasks != null ? DbSetHelpers.GetDbSetMock(tasks) : null;

            var taskDbC = new Mock<ITaskDbContext>();
            taskDbC.Setup(t => t.Tasks).Returns(taskDbSet?.Object);
            fixture.Register<ITaskDbContext>(() => taskDbC.Object);
        }
    }
}
