namespace Курсач.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ms6 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderStates", "order_OrderID", "dbo.Orders");
            DropIndex("dbo.OrderStates", new[] { "order_OrderID" });
            RenameColumn(table: "dbo.OrderStates", name: "order_OrderID", newName: "OrderID");
            DropPrimaryKey("dbo.OrderStates");
            AlterColumn("dbo.OrderStates", "OrderID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.OrderStates", new[] { "OrderID", "StateID" });
            CreateIndex("dbo.OrderStates", "OrderID");
            AddForeignKey("dbo.OrderStates", "OrderID", "dbo.Orders", "OrderID", cascadeDelete: true);
            DropColumn("dbo.OrderStates", "OrderD");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderStates", "OrderD", c => c.Int(nullable: false));
            DropForeignKey("dbo.OrderStates", "OrderID", "dbo.Orders");
            DropIndex("dbo.OrderStates", new[] { "OrderID" });
            DropPrimaryKey("dbo.OrderStates");
            AlterColumn("dbo.OrderStates", "OrderID", c => c.Int());
            AddPrimaryKey("dbo.OrderStates", new[] { "OrderD", "StateID" });
            RenameColumn(table: "dbo.OrderStates", name: "OrderID", newName: "order_OrderID");
            CreateIndex("dbo.OrderStates", "order_OrderID");
            AddForeignKey("dbo.OrderStates", "order_OrderID", "dbo.Orders", "OrderID");
        }
    }
}
