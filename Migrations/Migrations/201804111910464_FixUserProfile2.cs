namespace Migrations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixUserProfile2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserGoals", "UserProfile_Id", "dbo.UserProfiles");
            DropForeignKey("dbo.UserInterests", "UserProfile_Id", "dbo.UserProfiles");
            DropIndex("dbo.UserGoals", new[] { "UserProfile_Id" });
            DropIndex("dbo.UserInterests", new[] { "UserProfile_Id" });
            DropColumn("dbo.UserGoals", "UserProfileId");
            DropColumn("dbo.UserInterests", "UserProfileId");
            RenameColumn(table: "dbo.UserGoals", name: "UserProfile_Id", newName: "UserProfileId");
            RenameColumn(table: "dbo.UserInterests", name: "UserProfile_Id", newName: "UserProfileId");
            AlterColumn("dbo.UserGoals", "UserProfileId", c => c.Int(nullable: false));
            AlterColumn("dbo.UserGoals", "UserProfileId", c => c.Int(nullable: false));
            AlterColumn("dbo.UserInterests", "UserProfileId", c => c.Int(nullable: false));
            AlterColumn("dbo.UserInterests", "UserProfileId", c => c.Int(nullable: false));
            CreateIndex("dbo.UserGoals", "UserProfileId");
            CreateIndex("dbo.UserInterests", "UserProfileId");
            AddForeignKey("dbo.UserGoals", "UserProfileId", "dbo.UserProfiles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserInterests", "UserProfileId", "dbo.UserProfiles", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserInterests", "UserProfileId", "dbo.UserProfiles");
            DropForeignKey("dbo.UserGoals", "UserProfileId", "dbo.UserProfiles");
            DropIndex("dbo.UserInterests", new[] { "UserProfileId" });
            DropIndex("dbo.UserGoals", new[] { "UserProfileId" });
            AlterColumn("dbo.UserInterests", "UserProfileId", c => c.Int());
            AlterColumn("dbo.UserInterests", "UserProfileId", c => c.String(nullable: false));
            AlterColumn("dbo.UserGoals", "UserProfileId", c => c.Int());
            AlterColumn("dbo.UserGoals", "UserProfileId", c => c.String(nullable: false));
            RenameColumn(table: "dbo.UserInterests", name: "UserProfileId", newName: "UserProfile_Id");
            RenameColumn(table: "dbo.UserGoals", name: "UserProfileId", newName: "UserProfile_Id");
            AddColumn("dbo.UserInterests", "UserProfileId", c => c.String(nullable: false));
            AddColumn("dbo.UserGoals", "UserProfileId", c => c.String(nullable: false));
            CreateIndex("dbo.UserInterests", "UserProfile_Id");
            CreateIndex("dbo.UserGoals", "UserProfile_Id");
            AddForeignKey("dbo.UserInterests", "UserProfile_Id", "dbo.UserProfiles", "Id");
            AddForeignKey("dbo.UserGoals", "UserProfile_Id", "dbo.UserProfiles", "Id");
        }
    }
}
