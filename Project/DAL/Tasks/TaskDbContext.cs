using Microsoft.AspNet.Identity.EntityFramework;
using Project.Models.Account;
using Project.Models.Tasks;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Project.DAL.Tasks {

    public interface ITaskDbContext {
        DbSet<Task> Tasks { get; set; }

    }

    public class TaskDbContext : DbContext, ITaskDbContext {

        public TaskDbContext() : base("DefaultConnection") { }
        public TaskDbContext(string connectionString) : base(connectionString) { }


        public DbSet<Task> Tasks { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUserRole>()
                .HasKey(r => new { r.UserId, r.RoleId })
                .ToTable("AspNetUserRoles");

            modelBuilder.Entity<IdentityUserLogin>()
                .HasKey(u => new { u.LoginProvider, u.ProviderKey, u.UserId })
                .ToTable("AspNetUserLogins");

            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}