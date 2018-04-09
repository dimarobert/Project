namespace Migrations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixLikesRelations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Likes", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Likes", "StoryId");
            CreateIndex("dbo.Likes", "UserId");
            AddForeignKey("dbo.Likes", "StoryId", "dbo.Stories", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Likes", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Likes", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Likes", "StoryId", "dbo.Stories");
            DropIndex("dbo.Likes", new[] { "UserId" });
            DropIndex("dbo.Likes", new[] { "StoryId" });
            AlterColumn("dbo.Likes", "UserId", c => c.String());
        }
    }
}
