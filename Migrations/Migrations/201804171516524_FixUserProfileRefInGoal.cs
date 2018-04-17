namespace Migrations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixUserProfileRefInGoal : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Goals", "UserProfileId", "dbo.UserProfiles");
            AddForeignKey("dbo.Goals", "UserProfileId", "dbo.UserProfiles", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Goals", "UserProfileId", "dbo.UserProfiles");
            AddForeignKey("dbo.Goals", "UserProfileId", "dbo.UserProfiles", "Id", cascadeDelete: true);
        }
    }
}
