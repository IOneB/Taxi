namespace Курсач.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ms5 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.StateOrders", "State_StateID", "dbo.States");
            DropForeignKey("dbo.StateOrders", "Order_OrderID", "dbo.Orders");
            DropIndex("dbo.StateOrders", new[] { "State_StateID" });
            DropIndex("dbo.StateOrders", new[] { "Order_OrderID" });
            CreateTable(
                "dbo.OrderStates",
                c => new
                    {
                        OrderD = c.Int(nullable: false),
                        StateID = c.Int(nullable: false),
                        date = c.DateTime(nullable: false),
                        order_OrderID = c.Int(),
                    })
                .PrimaryKey(t => new { t.OrderD, t.StateID })
                .ForeignKey("dbo.Orders", t => t.order_OrderID)
                .ForeignKey("dbo.States", t => t.StateID, cascadeDelete: true)
                .Index(t => t.StateID)
                .Index(t => t.order_OrderID);
            
            DropTable("dbo.StateOrders");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.StateOrders",
                c => new
                    {
                        State_StateID = c.Int(nullable: false),
                        Order_OrderID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.State_StateID, t.Order_OrderID });
            
            DropForeignKey("dbo.OrderStates", "StateID", "dbo.States");
            DropForeignKey("dbo.OrderStates", "order_OrderID", "dbo.Orders");
            DropIndex("dbo.OrderStates", new[] { "order_OrderID" });
            DropIndex("dbo.OrderStates", new[] { "StateID" });
            DropTable("dbo.OrderStates");
            CreateIndex("dbo.StateOrders", "Order_OrderID");
            CreateIndex("dbo.StateOrders", "State_StateID");
            AddForeignKey("dbo.StateOrders", "Order_OrderID", "dbo.Orders", "OrderID", cascadeDelete: true);
            AddForeignKey("dbo.StateOrders", "State_StateID", "dbo.States", "StateID", cascadeDelete: true);
        }
    }
}
