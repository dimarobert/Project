namespace Migrations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserProfile : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProfiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        BirthDate = c.DateTime(nullable: false),
                        AboutMe = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            AddColumn("dbo.UserGoals", "UserProfile_Id", c => c.Int());
            AddColumn("dbo.UserInterests", "UserProfile_Id", c => c.Int());
            CreateIndex("dbo.UserGoals", "UserProfile_Id");
            CreateIndex("dbo.UserInterests", "UserProfile_Id");
            AddForeignKey("dbo.UserGoals", "UserProfile_Id", "dbo.UserProfiles", "Id");
            AddForeignKey("dbo.UserInterests", "UserProfile_Id", "dbo.UserProfiles", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserProfiles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserInterests", "UserProfile_Id", "dbo.UserProfiles");
            DropForeignKey("dbo.UserGoals", "UserProfile_Id", "dbo.UserProfiles");
            DropIndex("dbo.UserProfiles", new[] { "UserId" });
            DropIndex("dbo.UserInterests", new[] { "UserProfile_Id" });
            DropIndex("dbo.UserGoals", new[] { "UserProfile_Id" });
            DropColumn("dbo.UserInterests", "UserProfile_Id");
            DropColumn("dbo.UserGoals", "UserProfile_Id");
            DropTable("dbo.UserProfiles");
        }
    }
}
