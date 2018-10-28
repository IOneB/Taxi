using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Курсач.Models
{
    public class PageInfo
    {
        public int PageNumber { get; set; } // номер текущей страницы
        public int PageSize { get; set; } // кол-во объектов на странице
        public int TotalItems { get; set; } // всего объектов
        public int TotalPages  // всего страниц
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / PageSize); }
        }
    }
    public class ViewModel
    {
        public IEnumerable<Colour> Colours { get; set; }
        public IEnumerable<ApplicationUser> Users { get; set; }
        public IEnumerable<Order> Orders { get; set; }
        public IEnumerable<Driver> Drivers { get; set; }
        public IEnumerable<Discount> Discounts { get; set; }
        public IEnumerable<Car> Cars { get; set; }
        public IEnumerable<Brand> Brands { get; set; }
        public List<State> States { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}