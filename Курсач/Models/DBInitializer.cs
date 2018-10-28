using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Курсач.Models
{
    public class DBInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext db)
        {
            db.Colours.Add(new Colour { ColourName = "Красный" });
            db.Colours.Add(new Colour { ColourName = "Синий" });
            db.Colours.Add(new Colour { ColourName = "Желтый" });

            base.Seed(db);
        }
    }
}