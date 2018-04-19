namespace Migrations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGroupModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Goals", "UserProfile_Id", "dbo.UserProfiles");
            DropIndex("dbo.Goals", new[] { "UserProfileId" });
            DropIndex("dbo.Goals", new[] { "UserProfile_Id" });
            DropColumn("dbo.Goals", "UserProfileId");
            RenameColumn(table: "dbo.Goals", name: "UserProfile_Id", newName: "UserProfileId");
            AlterColumn("dbo.Goals", "UserProfileId", c => c.Int(nullable: false));
            CreateIndex("dbo.Goals", "UserProfileId");
            AddForeignKey("dbo.Goals", "UserProfileId", "dbo.UserProfiles", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Goals", "UserProfileId", "dbo.UserProfiles");
            DropIndex("dbo.Goals", new[] { "UserProfileId" });
            AlterColumn("dbo.Goals", "UserProfileId", c => c.Int());
            RenameColumn(table: "dbo.Goals", name: "UserProfileId", newName: "UserProfile_Id");
            AddColumn("dbo.Goals", "UserProfileId", c => c.Int(nullable: false));
            CreateIndex("dbo.Goals", "UserProfile_Id");
            CreateIndex("dbo.Goals", "UserProfileId");
            AddForeignKey("dbo.Goals", "UserProfile_Id", "dbo.UserProfiles", "Id");
        }
    }
}
