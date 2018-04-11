namespace Migrations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixUserGoals : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserGoals", "Goal_Id", "dbo.Goals");
            DropIndex("dbo.UserGoals", new[] { "Goal_Id" });
            RenameColumn(table: "dbo.UserGoals", name: "Goal_Id", newName: "GoalId");
            AlterColumn("dbo.UserGoals", "GoalId", c => c.Int(nullable: false));
            CreateIndex("dbo.UserGoals", "GoalId");
            AddForeignKey("dbo.UserGoals", "GoalId", "dbo.Goals", "Id", cascadeDelete: true);
            DropColumn("dbo.UserGoals", "HobbyId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserGoals", "HobbyId", c => c.Int(nullable: false));
            DropForeignKey("dbo.UserGoals", "GoalId", "dbo.Goals");
            DropIndex("dbo.UserGoals", new[] { "GoalId" });
            AlterColumn("dbo.UserGoals", "GoalId", c => c.Int());
            RenameColumn(table: "dbo.UserGoals", name: "GoalId", newName: "Goal_Id");
            CreateIndex("dbo.UserGoals", "Goal_Id");
            AddForeignKey("dbo.UserGoals", "Goal_Id", "dbo.Goals", "Id");
        }
    }
}
