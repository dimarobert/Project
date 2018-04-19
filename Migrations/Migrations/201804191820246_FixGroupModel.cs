namespace Migrations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixGroupModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GroupMembers", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.GroupMembers", "UserProfileId_Id", "dbo.UserProfiles");
            DropIndex("dbo.GroupMembers", new[] { "Group_Id" });
            DropIndex("dbo.GroupMembers", new[] { "UserProfileId_Id" });
            RenameColumn(table: "dbo.GroupMembers", name: "Group_Id", newName: "GroupId");
            RenameColumn(table: "dbo.GroupMembers", name: "UserProfileId_Id", newName: "UserProfileId");
            AlterColumn("dbo.GroupMembers", "GroupId", c => c.Int(nullable: false));
            AlterColumn("dbo.GroupMembers", "UserProfileId", c => c.Int(nullable: false));
            CreateIndex("dbo.GroupMembers", "GroupId");
            CreateIndex("dbo.GroupMembers", "UserProfileId");
            AddForeignKey("dbo.GroupMembers", "GroupId", "dbo.Groups", "Id", cascadeDelete: true);
            AddForeignKey("dbo.GroupMembers", "UserProfileId", "dbo.UserProfiles", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GroupMembers", "UserProfileId", "dbo.UserProfiles");
            DropForeignKey("dbo.GroupMembers", "GroupId", "dbo.Groups");
            DropIndex("dbo.GroupMembers", new[] { "UserProfileId" });
            DropIndex("dbo.GroupMembers", new[] { "GroupId" });
            AlterColumn("dbo.GroupMembers", "UserProfileId", c => c.Int());
            AlterColumn("dbo.GroupMembers", "GroupId", c => c.Int());
            RenameColumn(table: "dbo.GroupMembers", name: "UserProfileId", newName: "UserProfileId_Id");
            RenameColumn(table: "dbo.GroupMembers", name: "GroupId", newName: "Group_Id");
            CreateIndex("dbo.GroupMembers", "UserProfileId_Id");
            CreateIndex("dbo.GroupMembers", "Group_Id");
            AddForeignKey("dbo.GroupMembers", "UserProfileId_Id", "dbo.UserProfiles", "Id");
            AddForeignKey("dbo.GroupMembers", "Group_Id", "dbo.Groups", "Id");
        }
    }
}
