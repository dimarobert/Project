using Project.DAL.Tasks;
using Project.Models.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Repositories.Tasks {

    public interface ITaskRepository {
        IList<Task> GetUserTasks(string userId);

    }

    public class TaskRepository : ITaskRepository {

        ITaskDbContext TaskDbContext { get; }

        public TaskRepository(ITaskDbContext taskDbContext) {
            TaskDbContext = taskDbContext ?? throw new ArgumentNullException(nameof(taskDbContext));
        }

        public IList<Task> GetUserTasks(string userId) {
            if (string.IsNullOrWhiteSpace(userId) || TaskDbContext.Tasks == null)
                return new List<Task>();

            return TaskDbContext.Tasks.Where(task => task.UserId == userId).ToList();
        }
    }
}