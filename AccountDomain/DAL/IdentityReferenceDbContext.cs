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
    public abstract class IdentityReferenceDbContext<TContext> : BaseDbContext<TContext> where TContext : DbContext {
        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new UserInfoTypeConfiguration());
            modelBuilder.Configurations.Add(new RoleInfoTypeConfiguration());
            modelBuilder.Configurations.Add(new UserRoleInfoTypeConfiguration());

            modelBuilder.Configurations.Add(new IdentityUserLoginTypeConfiguration());
            modelBuilder.Configurations.Add(new IdentityUserClaimsTypeConfiguration());
        }
    }

    public class UserInfoTypeConfiguration : EntityTypeConfiguration<UserInfo> {
        public UserInfoTypeConfiguration() {
            HasKey(u => new { u.Id });

            HasMany(u => u.Roles)
                .WithRequired()
                .HasForeignKey(r => r.UserId);

            HasMany(u => u.Logins)
                .WithRequired()
                .HasForeignKey(l => l.UserId);

            HasMany(u => u.Claims)
                .WithRequired()
                .HasForeignKey(c => c.UserId);

            ToTable("AspNetUsers");
        }
    }

    public class RoleInfoTypeConfiguration : EntityTypeConfiguration<RoleInfo> {
        public RoleInfoTypeConfiguration() {
            HasKey(r => new { r.Id })
            .ToTable("AspNetRoles");
        }
    }

    public class UserRoleInfoTypeConfiguration : EntityTypeConfiguration<UserRoleInfo> {
        public UserRoleInfoTypeConfiguration() {
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

    public class IdentityUserClaimsTypeConfiguration : EntityTypeConfiguration<IdentityUserClaim> {
        public IdentityUserClaimsTypeConfiguration() {
            HasKey(l => new { l.Id })
            .ToTable("AspNetUserClaims");
        }
    }
}
