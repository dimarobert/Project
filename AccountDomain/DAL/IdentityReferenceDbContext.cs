using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Core.DbContext;
using Project.Account.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Project.Account.DAL {
    public abstract class IdentityReferenceDbContext<TContext> : BaseDbContext<TContext> where TContext : System.Data.Entity.DbContext {
        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new IdentityUserRoleTypeConfiguration());
            modelBuilder.Configurations.Add(new IdentityUserLoginTypeConfiguration());
        }
    }

    public class IdentityUserRoleTypeConfiguration : EntityTypeConfiguration<IdentityUserRole> {
        public IdentityUserRoleTypeConfiguration() {
            HasKey(r => new { r.UserId, r.RoleId })
            .ToTable("AspNetUserRoles");
        }
    }

    public class IdentityUserLoginTypeConfiguration : EntityTypeConfiguration<IdentityUserLogin> {
        public IdentityUserLoginTypeConfiguration() {
            HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId })
            .ToTable("AspNetUserLogins");
        }
    }
}
