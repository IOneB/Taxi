using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Курсач.Models;
using System.Data.Entity;
namespace Курсач.Controllers
{
    public class DriverController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private object o = new object();
        [Authorize(Roles = "Driver")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Same(int Number = 0,string actionName = "")
        {
            if (actionName != "")
            {
                Order order = db.Orders.Where(o => o.OrderID == Number).FirstOrDefault();
                if (order == null)
                {

                }
                else if (actionName == "done")
                {
                        if (order.States.OrderBy(s => s.date).ToList().Last().state.StateName != "Завершен")
                        {
                            order.SystemEndTime = DateTime.Now;
                        var orderState = new OrderState { OrderID = order.OrderID, StateID = 3, date = DateTime.Now, user = User.Identity.Name };
                            db.OrderStates.Add(orderState);
                            db.SaveChanges();
                        }
                }
                else if (actionName == "cancel")
                {
                        if (order.States.OrderBy(s => s.date).ToList().Last().state.StateName != "Отменен")
                        {
                            order.SystemEndTime = DateTime.Now;
                            var orderState = new OrderState { OrderID = order.OrderID, StateID = 4, date = DateTime.Now, user = User.Identity.Name };
                            db.OrderStates.Add(orderState);
                            db.SaveChanges();
                        }
                }
            }
            return View("Index");
        }

        [Authorize(Roles = "Driver")]
        public ActionResult FreeOrdersPartial(int id=0)
        {
            string user = HttpContext.User.Identity.Name;
            List<Order> currentOrderTemp = new List<Order>();
            currentOrderTemp = CONVERTER3000.TRANSFORMATION(db.Orders.Where(o =>o.Driver.User.UserName == user).ToList());
            bool isCurrentOrder = false;
            List<Order> currentOrder = new List<Order>(); 
            foreach (Order order in currentOrderTemp)
            {
                if (order.States.OrderBy(s=>s.date).ToList().Last().state.StateName=="Выполняется")
                {
                    isCurrentOrder = true;
                    currentOrder.Add(order);
                    break;
                }
            }
            if (isCurrentOrder)
            {
                ViewBag.Order = true;
                return PartialView(currentOrder);
            }
            else
            {
                if (id != 0)
                {
                    lock (o)
                    {
                        Order order = db.Orders.Where(o => o.OrderID == id).FirstOrDefault();
                        Driver driver = db.Drivers.Include(d => d.User).Where(d => d.User.UserName == user).FirstOrDefault();
                        if (order != null && order.DriverID != driver.DriverID && order.States.OrderBy(s => s.date).ToList().Last().state.StateName == "Выполняется")
                            ViewBag.Error = "Заказ уже взят другим водителем. Thank you Mario, but your princess is in another castle!";
                        else if (order.States.OrderBy(s => s.date).ToList().Last().state.StateName != "Выполняется")
                        {
                            ViewBag.Order = true;
                            order.DriverID = driver.DriverID;
                            order.Driver = driver;
                            var orderState = new OrderState { OrderID = order.OrderID, StateID = 2, date = DateTime.Now, user = User.Identity.Name };
                            db.OrderStates.Add(orderState);
                            List<Order> list = new List<Order>
                        {
                            order
                        };
                            db.SaveChanges();
                            list = CONVERTER3000.TRANSFORMATION(list);
                            return PartialView(list);
                        }
                    }
                }
                ViewBag.Order = false;
                List<Order> freeOrdersTemp = CONVERTER3000.TRANSFORMATION(db.Orders.Where(o => o.ApplicationUser.UserName!=user ).ToList());
                List<Order> freeOrders = new List<Order>();
                foreach (Order order in freeOrdersTemp)
                {
                    string stateName = order.States.OrderBy(s => s.date).ToList().Last().state.StateName;
                    if ((stateName=="Принят" || stateName=="Отменен") && (DateTime.Now - order.StartTime).Days >= 1)
                    {
                        var orderState = new OrderState { OrderID = order.OrderID, StateID = 5, date = DateTime.Now, user = "System" };
                        db.OrderStates.Add(orderState);
                        db.SaveChanges();
                        stateName = "Истек срок";
                    }
                    if ( stateName == "Принят" || stateName == "Отменен")
                    {
                            freeOrders.Add(order);
                    }
                }
                return PartialView(freeOrders);
            }
        }
        [Authorize(Roles = "Driver")]
        public ActionResult SuperMap()
        {
            ViewBag.Url = "https://www.google.com/maps/embed/v1/place?key=AIzaSyAIiadJY-IXcTyRDxo9QOPJ0QbVY5eHDlc&q=%D0%A5%D0%B0%D0%B1%D0%B0%D1%80%D0%BE%D0%B2%D1%81%D0%BA";
            return PartialView();
        }
        [Authorize(Roles = "Driver")]
        public ActionResult ReturnMap(int id)
        {
            Order order = db.Orders.Where(o => o.OrderID == id).FirstOrDefault();
            if (order==null)
            {
                ViewBag.Url = "https://www.google.com/maps/embed/v1/place?key=AIzaSyAIiadJY-IXcTyRDxo9QOPJ0QbVY5eHDlc&q=%D0%A5%D0%B0%D0%B1%D0%B0%D1%80%D0%BE%D0%B2%D1%81%D0%BA";
                return PartialView("SuperMap");
            }
            string url = "https://www.google.com/maps/embed/v1/directions?key=AIzaSyAIiadJY-IXcTyRDxo9QOPJ0QbVY5eHDlc&";
            url += $"origin={Uri.EscapeDataString(order.StartGPS)}";
            if (order.IntermediateGPS1!="null")
            {
                url += $"&waypoints={Uri.EscapeDataString(order.IntermediateGPS1)}";
                if (order.IntermediateGPS2!="null")
                {
                    url += $"|{Uri.EscapeDataString(order.IntermediateGPS2)}";
                    if (order.IntermediateGPS3 != "null")
                        url += $"|{Uri.EscapeDataString(order.IntermediateGPS3)}";
                }
            }
            url += $"&destination={Uri.EscapeDataString(order.EndGPS)}";
            ViewBag.Url = url;
            return PartialView("SuperMap");
        }
    }
}