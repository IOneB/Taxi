namespace Курсач.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.OrderStates");
            AddPrimaryKey("dbo.OrderStates", new[] { "date", "OrderID", "StateID" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.OrderStates");
            AddPrimaryKey("dbo.OrderStates", new[] { "OrderID", "StateID" });
        }
    }
}
