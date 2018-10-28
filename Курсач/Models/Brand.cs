using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Курсач.Models
{
    public class Brand
    {
        public int BrandID { get; set; }
        //Наименование марки автомобиля
        public string BrandName { get; set; }
        //Автомобили данной марки
        public List<Car> Cars { get; set; }
    }
}