namespace Project.Migrations.Tasks
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class TaskDbConfiguration : DbMigrationsConfiguration<Project.DAL.Tasks.TaskDbContext>
    {
        public TaskDbConfiguration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\Tasks";
            ContextKey = "TaskDbConfiguration";
        }

        protected override void Seed(Project.DAL.Tasks.TaskDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
