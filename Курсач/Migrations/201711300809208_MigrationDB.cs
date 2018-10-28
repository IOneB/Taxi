namespace Курсач.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationDB : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Orders", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.Orders", "ApplicationUserID");
            RenameColumn(table: "dbo.Orders", name: "ApplicationUser_Id", newName: "ApplicationUserID");
            AlterColumn("dbo.Orders", "ApplicationUserID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Orders", "ApplicationUserID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Orders", new[] { "ApplicationUserID" });
            AlterColumn("dbo.Orders", "ApplicationUserID", c => c.Int());
            RenameColumn(table: "dbo.Orders", name: "ApplicationUserID", newName: "ApplicationUser_Id");
            AddColumn("dbo.Orders", "ApplicationUserID", c => c.Int());
            CreateIndex("dbo.Orders", "ApplicationUser_Id");
        }
    }
}
