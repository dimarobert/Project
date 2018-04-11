namespace Migrations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixUserProfile : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserGoals", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserInterests", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserGoals", new[] { "UserId" });
            DropIndex("dbo.UserInterests", new[] { "UserId" });
            AddColumn("dbo.UserGoals", "UserProfileId", c => c.String(nullable: false));
            AddColumn("dbo.UserInterests", "UserProfileId", c => c.String(nullable: false));
            DropColumn("dbo.UserGoals", "UserId");
            DropColumn("dbo.UserInterests", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserInterests", "UserId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.UserGoals", "UserId", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.UserInterests", "UserProfileId");
            DropColumn("dbo.UserGoals", "UserProfileId");
            CreateIndex("dbo.UserInterests", "UserId");
            CreateIndex("dbo.UserGoals", "UserId");
            AddForeignKey("dbo.UserInterests", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserGoals", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
