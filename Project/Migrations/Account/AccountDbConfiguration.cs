namespace Project.Migrations.Account
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class AccountDbConfiguration : DbMigrationsConfiguration<Project.Models.Account.AccountDbContext>
    {
        public AccountDbConfiguration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\Account";
            ContextKey = "AccountDbConfiguration";
        }

        protected override void Seed(Project.Models.Account.AccountDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
