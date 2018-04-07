namespace Migrations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInterestsAndHobbies : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Hobbies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Interests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserHobbies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HobbyId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Hobbies", t => t.HobbyId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.HobbyId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserInterests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InterestId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Interests", t => t.InterestId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.InterestId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserInterests", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserInterests", "InterestId", "dbo.Interests");
            DropForeignKey("dbo.UserHobbies", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserHobbies", "HobbyId", "dbo.Hobbies");
            DropIndex("dbo.UserInterests", new[] { "UserId" });
            DropIndex("dbo.UserInterests", new[] { "InterestId" });
            DropIndex("dbo.UserHobbies", new[] { "UserId" });
            DropIndex("dbo.UserHobbies", new[] { "HobbyId" });
            DropTable("dbo.UserInterests");
            DropTable("dbo.UserHobbies");
            DropTable("dbo.Interests");
            DropTable("dbo.Hobbies");
        }
    }
}
