namespace Migrations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeUserRequiredForInterestsAndHobbies : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserHobbies", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserInterests", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserHobbies", new[] { "UserId" });
            DropIndex("dbo.UserInterests", new[] { "UserId" });
            AlterColumn("dbo.UserHobbies", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.UserInterests", "UserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.UserHobbies", "UserId");
            CreateIndex("dbo.UserInterests", "UserId");
            AddForeignKey("dbo.UserHobbies", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserInterests", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserInterests", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserHobbies", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserInterests", new[] { "UserId" });
            DropIndex("dbo.UserHobbies", new[] { "UserId" });
            AlterColumn("dbo.UserInterests", "UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.UserHobbies", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.UserInterests", "UserId");
            CreateIndex("dbo.UserHobbies", "UserId");
            AddForeignKey("dbo.UserInterests", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.UserHobbies", "UserId", "dbo.AspNetUsers", "Id");
        }
    }
}
