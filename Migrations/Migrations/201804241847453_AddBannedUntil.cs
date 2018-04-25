namespace Migrations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBannedUntil : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfiles", "BannedUntil", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfiles", "BannedUntil");
        }
    }
}
