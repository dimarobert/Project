using Project.DAL.Tasks;
using Project.Models.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Managers.Tasks {

    public interface ITaskManager {
        IList<Task> GetUserTasks(string userId);

    }

    public class TaskManager : ITaskManager {

        ITaskDbContext TaskDbContext { get; }

        public TaskManager(ITaskDbContext taskDbContext) {
            TaskDbContext = taskDbContext;
        }

        public IList<Task> GetUserTasks(string userId) {
            return TaskDbContext.Tasks.Where(task => task.UserId == userId).ToList();
        }
    }
}