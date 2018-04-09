namespace Migrations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameHobbyToGoal : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "ParentStoryId", "dbo.Stories");
            DropForeignKey("dbo.Comments", "ParentCommentId", "dbo.Comments");
            RenameTable(name: "dbo.Hobbies", newName: "Goals");
            DropForeignKey("dbo.UserHobbies", "HobbyId", "dbo.Hobbies");
            DropForeignKey("dbo.UserHobbies", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserHobbies", new[] { "HobbyId" });
            DropIndex("dbo.UserHobbies", new[] { "UserId" });
            CreateTable(
                "dbo.UserGoals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HobbyId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        Goal_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Goals", t => t.Goal_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.Goal_Id);
            
            AddForeignKey("dbo.Comments", "ParentCommentId", "dbo.Comments", "Id");
            AddForeignKey("dbo.Comments", "ParentStoryId", "dbo.Stories", "Id", cascadeDelete: true);
            DropTable("dbo.UserHobbies");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserHobbies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HobbyId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.UserGoals", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserGoals", "Goal_Id", "dbo.Goals");
            DropForeignKey("dbo.Comments", "ParentStoryId", "dbo.Stories");
            DropForeignKey("dbo.Comments", "ParentCommentId", "dbo.Comments");
            DropIndex("dbo.UserGoals", new[] { "Goal_Id" });
            DropIndex("dbo.UserGoals", new[] { "UserId" });
            DropTable("dbo.UserGoals");
            CreateIndex("dbo.UserHobbies", "UserId");
            CreateIndex("dbo.UserHobbies", "HobbyId");
            AddForeignKey("dbo.UserHobbies", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserHobbies", "HobbyId", "dbo.Hobbies", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.Goals", newName: "Hobbies");
        }
    }
}
