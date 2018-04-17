namespace Migrations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GoalsAndStepsMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserGoals", "GoalId", "dbo.Goals");
            DropIndex("dbo.UserGoals", new[] { "GoalId" });
            DropIndex("dbo.UserGoals", new[] { "UserProfileId" });
            CreateTable(
                "dbo.Steps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        GoalId = c.Int(nullable: false),
                        IsDone = c.Boolean(nullable: false),
                        Difficulty = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Goals", t => t.GoalId, cascadeDelete: true)
                .Index(t => t.GoalId);
            
            AddColumn("dbo.Stories", "Type", c => c.Int(nullable: false));
            AddColumn("dbo.Goals", "UserProfileId", c => c.Int(nullable: false));
            CreateIndex("dbo.Goals", "UserProfileId");
            DropTable("dbo.UserGoals");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserGoals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GoalId = c.Int(nullable: false),
                        UserProfileId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Steps", "GoalId", "dbo.Goals");
            DropIndex("dbo.Steps", new[] { "GoalId" });
            DropIndex("dbo.Goals", new[] { "UserProfileId" });
            DropColumn("dbo.Goals", "UserProfileId");
            DropColumn("dbo.Stories", "Type");
            DropTable("dbo.Steps");
            CreateIndex("dbo.UserGoals", "UserProfileId");
            CreateIndex("dbo.UserGoals", "GoalId");
            AddForeignKey("dbo.UserGoals", "GoalId", "dbo.Goals", "Id", cascadeDelete: true);
        }
    }
}
