namespace Migrations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeBirthDateToNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserProfiles", "BirthDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserProfiles", "BirthDate", c => c.DateTime(nullable: false));
        }
    }
}
