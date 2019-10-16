namespace Курсач.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modelMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Brands",
                c => new
                    {
                        BrandID = c.Int(nullable: false, identity: true),
                        BrandName = c.String(),
                    })
                .PrimaryKey(t => t.BrandID);
            
            CreateTable(
                "dbo.Cars",
                c => new
                    {
                        CarID = c.Int(nullable: false, identity: true),
                        CarNumber = c.String(),
                        BrandID = c.Int(),
                        ColourID = c.Int(),
                        ReleaseYear = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CarID)
                .ForeignKey("dbo.Brands", t => t.BrandID)
                .ForeignKey("dbo.Colours", t => t.ColourID)
                .Index(t => t.BrandID)
                .Index(t => t.ColourID);
            
            CreateTable(
                "dbo.Colours",
                c => new
                    {
                        ColourID = c.Int(nullable: false, identity: true),
                        ColourName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ColourID);
            
            CreateTable(
                "dbo.Drivers",
                c => new
                    {
                        DriverID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Patronymic = c.String(),
                        BirthDate = c.DateTime(nullable: false),
                        Experience = c.Int(nullable: false),
                        CarID = c.Int(nullable: false),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.DriverID)
                .ForeignKey("dbo.Cars", t => t.CarID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.CarID)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        DriverID = c.Int(),
                        SystemStartTime = c.DateTime(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        SystemEndTime = c.DateTime(nullable: false),
                        IsChild = c.Boolean(nullable: false),
                        IsOptimised = c.Boolean(nullable: false),
                        IsShipment = c.Boolean(nullable: false),
                        ApplicationUserID = c.String(maxLength: 128),
                        StartGPS = c.String(),
                        IntermediateGPS1 = c.String(),
                        IntermediateGPS2 = c.String(),
                        IntermediateGPS3 = c.String(),
                        EndGPS = c.String(),
                        Distance = c.Double(nullable: false),
                        Cost = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.OrderID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.Drivers", t => t.DriverID)
                .Index(t => t.DriverID)
                .Index(t => t.ApplicationUserID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DiscountID = c.Int(),
                        FIO = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Discounts", t => t.DiscountID)
                .Index(t => t.DiscountID)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Discounts",
                c => new
                    {
                        DiscountID = c.Int(nullable: false, identity: true),
                        Size = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Reason = c.String(),
                    })
                .PrimaryKey(t => t.DiscountID);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.OrderStates",
                c => new
                    {
                        date = c.DateTime(nullable: false),
                        OrderID = c.Int(nullable: false),
                        StateID = c.Int(nullable: false),
                        user = c.String(),
                    })
                .PrimaryKey(t => new { t.date, t.OrderID, t.StateID })
                .ForeignKey("dbo.Orders", t => t.OrderID, cascadeDelete: true)
                .ForeignKey("dbo.States", t => t.StateID, cascadeDelete: true)
                .Index(t => t.OrderID)
                .Index(t => t.StateID);
            
            CreateTable(
                "dbo.States",
                c => new
                    {
                        StateID = c.Int(nullable: false, identity: true),
                        StateName = c.String(),
                    })
                .PrimaryKey(t => t.StateID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Drivers", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.OrderStates", "StateID", "dbo.States");
            DropForeignKey("dbo.OrderStates", "OrderID", "dbo.Orders");
            DropForeignKey("dbo.Orders", "DriverID", "dbo.Drivers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Orders", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "DiscountID", "dbo.Discounts");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Drivers", "CarID", "dbo.Cars");
            DropForeignKey("dbo.Cars", "ColourID", "dbo.Colours");
            DropForeignKey("dbo.Cars", "BrandID", "dbo.Brands");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.OrderStates", new[] { "StateID" });
            DropIndex("dbo.OrderStates", new[] { "OrderID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "DiscountID" });
            DropIndex("dbo.Orders", new[] { "ApplicationUserID" });
            DropIndex("dbo.Orders", new[] { "DriverID" });
            DropIndex("dbo.Drivers", new[] { "User_Id" });
            DropIndex("dbo.Drivers", new[] { "CarID" });
            DropIndex("dbo.Cars", new[] { "ColourID" });
            DropIndex("dbo.Cars", new[] { "BrandID" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.States");
            DropTable("dbo.OrderStates");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.Discounts");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Orders");
            DropTable("dbo.Drivers");
            DropTable("dbo.Colours");
            DropTable("dbo.Cars");
            DropTable("dbo.Brands");
        }
    }
}
