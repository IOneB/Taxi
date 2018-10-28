namespace Курсач.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Курсач.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Курсач.Models.ApplicationDbContext";
        }

        protected override void Seed(Курсач.Models.ApplicationDbContext context)
        {
            
        }
    }
}
