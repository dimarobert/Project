namespace Migrations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixCommentModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "ParentStoryId", "dbo.Stories");
            DropIndex("dbo.Comments", new[] { "ParentStoryId" });
            AlterColumn("dbo.Comments", "ParentStoryId", c => c.Int());
            CreateIndex("dbo.Comments", "ParentStoryId");
            AddForeignKey("dbo.Comments", "ParentStoryId", "dbo.Stories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "ParentStoryId", "dbo.Stories");
            DropIndex("dbo.Comments", new[] { "ParentStoryId" });
            AlterColumn("dbo.Comments", "ParentStoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.Comments", "ParentStoryId");
            AddForeignKey("dbo.Comments", "ParentStoryId", "dbo.Stories", "Id", cascadeDelete: true);
        }
    }
}
