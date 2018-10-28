using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;

namespace Курсач.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        //Водитель, обслуживающий заказ
        public Driver Driver { get; set; }
        public int? DriverID { get; set; }
        //Дата и время поступления заказа
        public DateTime SystemStartTime { get; set; }
        //Дата и время прибытия таксиста
        public DateTime StartTime { get; set; }
        //Время окончания окончания заказа
        public DateTime SystemEndTime { get; set; }
        //Услуга "Детское кресло"
        public bool IsChild { get; set; }
        // Услуга "Оптимизировать"
        public bool IsOptimised { get; set; }
        // Услуга "Грузоперевозки"
        public bool IsShipment { get; set; }
        //Клиент
        public string ApplicationUserID { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        //Стартовая позиция клиента
        public string StartGPS { get; set; }
        //Промежуточные точки
        public string IntermediateGPS1 { get; set; }
        public string IntermediateGPS2 { get; set; }
        public string IntermediateGPS3{ get; set; }
        //Пункт назначения
        public string EndGPS { get; set; }
        //Расстояние (для водителей)
        public double Distance { get; set; }
        //Стоимость заказа
        public double Cost { get; set; }
        //Состояние заказа
        public virtual ICollection<OrderState> States { get; set; }
    }
}