using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Курсач.Models;
using System.Net;
using System.Xml.Linq;
using System.Data.Entity;

namespace Курсач.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Tariffing()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        [Authorize(Roles = "Driver, User, Admin, Dispatcher")]
        [HttpPost]
        public ActionResult Order(string results="")
        {
            if (results == "none" || results == "")
            {
                ViewBag.Error = "Маршрут не был задан :с";
                return View("Error");
            }
            else
            {
                var order = results.Split('|');
                var start = order[0];
                var end = order[1];
                if (!CheckPoint(start) || !CheckPoint(end))
                {
                    ViewBag.Error = "Маршрут задан неверно";
                    return View("Error");
                }

                List<string> waypoints = new List<string>();
                for (int i = 0; i < 3; i++)
                {
                    if (order[i + 2] == "null")
                        break;
                    else
                    {
                        waypoints.Add(order[i + 2]);
                        if (!CheckPoint(waypoints[i]))
                        {
                            ViewBag.Error = "Маршрут задан неверно";
                            return View("Error");
                        }
                    }
                }
                var cost = Convert.ToDouble(order[5].Replace('.', ','));
                var distance = Convert.ToDouble(order[6].Replace('.', ','));
                var isOptimise = Convert.ToBoolean(order[7]);
                var isChild = Convert.ToBoolean(order[8]);
                var isSoon = Convert.ToBoolean(order[9]);
                var isShipment = Convert.ToBoolean(order[10]);
                DateTime dateTime;
                var currentUser = db.Users.Include(u=>u.Discount).Where(p => p.UserName == HttpContext.User.Identity.Name).FirstOrDefault();
                if (order.Length != 12)
                {
                    ViewBag.Error = "Ай-яй-яй";
                    return View("Error");
                }

                double routeDistance = CreateRoute(start,end,waypoints,isOptimise);
                if (routeDistance == -1)
                {
                    ViewBag.Error = "Не удалось построить маршрут";
                    return View("Error");
                }
                //else if (Math.Abs(routeDistance - distance) > 1)
                //{
                //    ViewBag.Error = "Расстояние было изменено, пересчитайте маршрут";
                //    return View("Error");
                //}
                double realCost = 100;
                if (!isSoon)
                    realCost += 50;
                if (isOptimise)
                    if (waypoints.Count > 1)
                        realCost += 50;
                if (isChild)
                    realCost += 50;
                if (isShipment)
                    realCost += 50;
                realCost += routeDistance * 10.0;
                //if (Math.Abs(cost - realCost) > 100)
                //{
                //    ViewBag.Error = "Стоимость была изменена, пересчитайте маршрут";
                //    return View("Error");
                //}
                if (order[order.Length - 1] != "")
                {
                    try
                    {
                        var dateTimeTemp = order[order.Length - 1].Split('T');
                        string[] date = dateTimeTemp[0].Split('-');
                        string[] time = dateTimeTemp[1].Split(':');
                        dateTime = new DateTime(Convert.ToInt32(date[0]), Convert.ToInt32(date[1]), Convert.ToInt32(date[2]), Convert.ToInt32(time[0]), Convert.ToInt32(time[1]), 0);
                    }
                    catch
                    {
                        ViewBag.Error = "Дата задана неверно";
                        return View("Error");
                    }
                }
                else { dateTime = DateTime.Now; }
                if (currentUser == null)
                {
                    ViewBag.Error = "Некорректные данные пользователя";
                    return View("Error");
                }
                double disco = 1;
                if (currentUser.Discount!=null)
                {
                    disco = 1 - (double)(currentUser.Discount.Size) / 100.0;
                }
                cost = cost * disco;
                Order newOrder = new Models.Order
                {
                    IsOptimised = isOptimise,
                    ApplicationUserID = currentUser.Id,
                    StartGPS = start,
                    EndGPS = end,
                    Cost = cost,
                    SystemStartTime = DateTime.Now,
                    StartTime = dateTime,
                    IsChild = isChild,
                    IsShipment = isShipment,
                    Distance = distance,
                    IntermediateGPS1 = order[2],
                    IntermediateGPS2 = order[3],
                    IntermediateGPS3 = order[4]
                };
                db.Orders.Add(newOrder);
                var orderState = new OrderState { order = newOrder, StateID = 1,date=DateTime.Now, user= currentUser.UserName };
                db.OrderStates.Add(orderState);
                db.SaveChanges();
                return View("Success");
            }
        }
        [Authorize(Roles = "Driver, User, Admin, Dispatcher")]
        public ActionResult DoOrder()
        {
            ViewBag.Title = "Order";
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        [Authorize(Roles = "Driver, User, Admin, Dispatcher")]
        private bool CheckPoint(string uri)
        {
            string requestUri = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?key=AIzaSyAIiadJY-IXcTyRDxo9QOPJ0QbVY5eHDlc&address={0}&sensor=false", Uri.EscapeDataString(uri));
            WebRequest request = WebRequest.Create(requestUri);
            WebResponse response = request.GetResponse();
            XDocument xdoc = XDocument.Load(response.GetResponseStream());
            XElement result = xdoc.Element("GeocodeResponse").Element("result");
            string state = xdoc.Element("GeocodeResponse").Element("status").Value;
            if (result == null || (state!="OK" && state!= "OVER_QUERY_LIMIT"))
                return false;
            return true;
        }
        [Authorize(Roles = "Driver, User, Admin, Dispatcher")]
        private double CreateRoute(string start, string end, List<string> waypoints, bool isOptimise)
        {
            string requestUri = string.Format("https://maps.googleapis.com/maps/api/directions/xml?origin={0}&destination={1}", Uri.EscapeDataString(start), Uri.EscapeDataString(end));
            if (waypoints.Count>0)
            {
                requestUri += string.Format("&waypoints=optimize:{0}", isOptimise);
                for (int i = 0; i < waypoints.Count; i++)
                {
                    requestUri += string.Format("|{0}",Uri.EscapeDataString(waypoints[i]));
                }
            }
            requestUri += string.Format("&key=AIzaSyAIiadJY-IXcTyRDxo9QOPJ0QbVY5eHDlc");
            WebRequest request = WebRequest.Create(requestUri);
            WebResponse response = request.GetResponse();
            XDocument xdoc = XDocument.Load(response.GetResponseStream());
            XElement route = xdoc.Element("DirectionsResponse").Element("route");
            double distance = 0;
            List<XElement> legs = route.Elements("leg").ToList();
            for (int i=0;i<legs.Count;i++)
            {
                distance += Convert.ToDouble(legs[i].Element("distance").Element("value").Value) / 1000.0;
            }
            if (route == null || xdoc.Element("DirectionsResponse").Element("status").Value != "OK")
                return -1;
            return distance;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}