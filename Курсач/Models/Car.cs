using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Курсач.Models
{
    public class Car
    {
        public int CarID { get; set; }
        //Гос номер авто
        public string CarNumber { get; set; }
        //Марка автомобиля
        public int? BrandID { get; set; }
        public Brand Brand { get; set; }
        //ID цвета автомобиля
        public int? ColourID { get; set; }
        public Colour Colour { get; set; }
        //Дата выпуска автомобиля
        public DateTime ReleaseYear { get; set; }
        //Водители, приписанные к данному автомобилю
        public List<Driver> Driver { get; set; }
    }
}