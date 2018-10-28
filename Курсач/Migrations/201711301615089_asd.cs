namespace Курсач.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asd : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Drivers", "Experience", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Drivers", "Experience", c => c.DateTime(nullable: false));
        }
    }
}
