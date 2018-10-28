namespace Курсач.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Optimised : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "IsOptimised", c => c.Boolean(nullable: false));
            DropColumn("dbo.Orders", "IsActive");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "IsActive", c => c.Boolean(nullable: false));
            DropColumn("dbo.Orders", "IsOptimised");
        }
    }
}
