namespace Migrations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StoryInterestGroup : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stories", "GroupId", c => c.Int());
            AddColumn("dbo.Groups", "InterestId", c => c.Int(nullable: false));
            CreateIndex("dbo.Stories", "GroupId");
            CreateIndex("dbo.Groups", "InterestId");
            AddForeignKey("dbo.Groups", "InterestId", "dbo.Interests", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Stories", "GroupId", "dbo.Groups", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Stories", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.Groups", "InterestId", "dbo.Interests");
            DropIndex("dbo.Groups", new[] { "InterestId" });
            DropIndex("dbo.Stories", new[] { "GroupId" });
            DropColumn("dbo.Groups", "InterestId");
            DropColumn("dbo.Stories", "GroupId");
        }
    }
}
