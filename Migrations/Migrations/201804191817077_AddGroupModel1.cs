namespace Migrations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGroupModel1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GroupMembers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Group_Id = c.Int(),
                        UserProfileId_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.Group_Id)
                .ForeignKey("dbo.UserProfiles", t => t.UserProfileId_Id)
                .Index(t => t.Group_Id)
                .Index(t => t.UserProfileId_Id);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GroupMembers", "UserProfileId_Id", "dbo.UserProfiles");
            DropForeignKey("dbo.GroupMembers", "Group_Id", "dbo.Groups");
            DropIndex("dbo.GroupMembers", new[] { "UserProfileId_Id" });
            DropIndex("dbo.GroupMembers", new[] { "Group_Id" });
            DropTable("dbo.Groups");
            DropTable("dbo.GroupMembers");
        }
    }
}
