namespace Курсач.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migr : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderStates", "user", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderStates", "user");
        }
    }
}
