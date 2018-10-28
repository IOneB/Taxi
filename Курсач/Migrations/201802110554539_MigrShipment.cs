namespace Курсач.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrShipment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "IsShipment", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "IsShipment");
        }
    }
}
