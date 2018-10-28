using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Курсач.Models
{
    public class Discount
    {
        public int DiscountID { get; set; }
        //Размер скидки
        public decimal Size { get; set; }
        //Причина
        public string Reason { get; set; }
        //Клиенты с данной скидкой
        public List<ApplicationUser> ApplicationUser { get; set; }
    }
}