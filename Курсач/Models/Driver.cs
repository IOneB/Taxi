using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Курсач.Models
{
    public class Driver
    {
        public int DriverID { get; set; }
        //Имя
        public string FirstName { get; set; }
        //Фамилия
        public string LastName { get; set; }
        //Отчество
        public string Patronymic { get; set; }
        //Дата рождения
        public DateTime BirthDate { get; set; }
        //Стаж водителя
        public int Experience { get; set; }
        //Автомобиль
        public int CarID { get; set; }
        public Car Car { get; set; }
        //Заказы, принятые данным водителем
        public List<Order> Orders { get; set; }
        //Связанный аккаунт
        [Required]
        public ApplicationUser User { get; set; }
    }
}