namespace Migrations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCommentDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "Date", c => c.DateTime());
            AddColumn("dbo.Stories", "Date", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stories", "Date");
            DropColumn("dbo.Comments", "Date");
        }
    }
}
