namespace Курсач.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateDB : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "SystemStartTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Orders", "SystemEndTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Orders", "IsChild", c => c.Boolean(nullable: false));
            DropColumn("dbo.Orders", "EndTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "EndTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Orders", "IsChild");
            DropColumn("dbo.Orders", "SystemEndTime");
            DropColumn("dbo.Orders", "SystemStartTime");
        }
    }
}
