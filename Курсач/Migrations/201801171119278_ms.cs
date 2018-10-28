namespace Курсач.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ms : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.States",
                c => new
                    {
                        StateID = c.Int(nullable: false, identity: true),
                        StateName = c.String(),
                    })
                .PrimaryKey(t => t.StateID);
            
            CreateTable(
                "dbo.StateOrders",
                c => new
                    {
                        State_StateID = c.Int(nullable: false),
                        Order_OrderID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.State_StateID, t.Order_OrderID })
                .ForeignKey("dbo.States", t => t.State_StateID, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.Order_OrderID, cascadeDelete: true)
                .Index(t => t.State_StateID)
                .Index(t => t.Order_OrderID);
            
            DropColumn("dbo.Orders", "State");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "State", c => c.String());
            DropForeignKey("dbo.StateOrders", "Order_OrderID", "dbo.Orders");
            DropForeignKey("dbo.StateOrders", "State_StateID", "dbo.States");
            DropIndex("dbo.StateOrders", new[] { "Order_OrderID" });
            DropIndex("dbo.StateOrders", new[] { "State_StateID" });
            DropTable("dbo.StateOrders");
            DropTable("dbo.States");
        }
    }
}
