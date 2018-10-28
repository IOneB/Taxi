namespace Курсач.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Order : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "IntermediateGPS1", c => c.String());
            AddColumn("dbo.Orders", "IntermediateGPS2", c => c.String());
            AddColumn("dbo.Orders", "IntermediateGPS3", c => c.String());
            AddColumn("dbo.Orders", "State", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "State");
            DropColumn("dbo.Orders", "IntermediateGPS3");
            DropColumn("dbo.Orders", "IntermediateGPS2");
            DropColumn("dbo.Orders", "IntermediateGPS1");
        }
    }
}
