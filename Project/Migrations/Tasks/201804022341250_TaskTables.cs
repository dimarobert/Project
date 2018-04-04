namespace Project.Migrations.Tasks
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaskTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserId);

            AddForeignKey("dbo.Tasks", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Tasks", new[] { "UserId" });
            DropTable("dbo.Tasks");
        }
    }
}
