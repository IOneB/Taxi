namespace Курсач.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationDB1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Drivers", "User_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Drivers", "User_Id");
            AddForeignKey("dbo.Drivers", "User_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Drivers", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Drivers", new[] { "User_Id" });
            DropColumn("dbo.Drivers", "User_Id");
        }
    }
}
