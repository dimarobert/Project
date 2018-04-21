namespace Migrations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateStoryModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stories", "Content", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stories", "Content");
        }
    }
}
