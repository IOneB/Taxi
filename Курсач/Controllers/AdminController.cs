using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Курсач.Models;
using System.Data.Entity;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Net.Http;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.Owin.Security;

namespace Курсач.Controllers
{
    public class AdminController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin
        [Authorize(Roles = "Admin")]
        public ActionResult Modering()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult CallForm(string name="colour", int? id = 0)
        {
            switch (name)
            {
                case "colour":
                    {
                        Colour Model = null;
                        if (id != 0)
                        {
                            Model = db.Colours.Where(p => p.ColourID == id).FirstOrDefault();
                        }
                        return PartialView("ColoursFormPartial", Model);
                    }
                case "brand":
                    {
                        Brand Model = null;
                        if (id != 0)
                        {
                            Model = db.Brands.Where(p => p.BrandID == id).FirstOrDefault();
                        }
                        return PartialView("BrandsFormPartial", Model);
                    }
                case "car":
                    {
                        List<Brand> Brands = db.Brands.OrderBy(b => b.BrandName).ToList();
                        ViewBag.Brands = Brands;
                        List<Colour> Colours = db.Colours.OrderBy(c => c.ColourName).ToList();
                        ViewBag.Colours = Colours;
                        Car Model = null;
                        if (id != 0)
                        {
                            Model = db.Cars.Where(c => c.CarID == id).FirstOrDefault();
                        }
                        return PartialView("CarsFormPartial", Model);                        
                    }
                case "driver":
                    {
                        List<Car> Cars = db.Cars.OrderBy(c => c.CarNumber).ToList();
                        ViewBag.Cars = Cars;
                        Driver Model = null;
                        return PartialView("DriversFormPartial", Model);
                    }
                case "discount":
                    {
                        Discount Model = null;
                        if (id != 0)
                        {
                            Model = db.Discounts.Where(p => p.DiscountID == id).FirstOrDefault();
                        }
                        return PartialView("DiscountsFormPartial", Model);
                    }
            }
            return PartialView("Error");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult UserDiscountFormPartial(string id)
        {
            if (db.Users.Where(u => u.Id == id).FirstOrDefault() == null)
            {
                id = db.Users.FirstOrDefault().Id;
            }
            ApplicationUser Model = db.Users.Include(u => u.Discount).ToList().Where(u => u.Id == id).FirstOrDefault();
            List<Discount> Discounts = db.Discounts.OrderBy(d => d.Reason).ToList();
            ViewBag.Discounts = Discounts;
            return PartialView("UserDiscountFormPartial", Model);
        }
        #region Формы
        [Authorize(Roles = "Admin")]
        public ActionResult SaveColour(string ColourName, int? id = 0)
        {
            Regex re = new Regex("[!@#$%^&*()_+`\\\"~[|/';:.,<>{}=?№ ]");
            if ((db.Colours.Count(p => p.ColourName == ColourName) > 0))
            {
                ViewBag.Error = "Данный цвет уже есть в базе";
            }
            else if(ColourName.Length<3 || re.Match(ColourName).Success || id < 0)
            {
                ViewBag.Error = "Ошибка сохранения, Ай-яй-яй";
            }
            else
            {
                if (id == 0)
                {
                    db.Colours.Add(new Colour { ColourName = ColourName });
                }
                else
                {
                    var colour = db.Colours.Where(p => p.ColourID == id).FirstOrDefault();
                    if (colour == null)
                        ViewBag.Error = "Такой записи не существует";
                    else
                    {
                        colour.ColourName = ColourName;
                    }
                }
                db.SaveChanges();
            }
            int page = 1;
            if (id == 0)
                page = (int)Math.Ceiling((decimal)db.Colours.Count() / 5);
            else
            {
                List<Colour> colours = db.Colours.ToList();
                int coloursIndex = colours.FindIndex(c => c.ColourID == id) + 1;
                page = (int)Math.Ceiling((decimal)coloursIndex / 5);
            }
            PartialViewResult result = Modering("colours", page, "") as PartialViewResult;
            return PartialView(result.ViewName, result.Model);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult SaveBrand(string BrandName, int? id = 0)
        {
            Regex re = new Regex("[!@#$%^&*()_+`\\\"~[|/';:.,<>{}=?№ ]");
            if ((db.Brands.Count(p => p.BrandName == BrandName) > 0))
            {
                ViewBag.Error = "Данный бренд уже определен";
            }
            else if(BrandName.Length < 3 || re.Match(BrandName).Success || id < 0)
            {
                ViewBag.Error = "Ошибка сохранения, Ай-яй-яй";
            }
            else
            {
                if (id == 0)
                {
                    db.Brands.Add(new Brand { BrandName = BrandName });
                }
                else
                {
                    var brand = db.Brands.Where(p => p.BrandID == id).FirstOrDefault();
                    if (brand == null)
                        ViewBag.Error = "Такой записи не существует";
                    else
                    {
                        brand.BrandName = BrandName;
                    }
                }
                db.SaveChanges();
            }
            int page = 1;
            if (id == 0)
                page = (int)Math.Ceiling((decimal)db.Brands.Count() / 5);
            else
            {
                List<Brand> brands = db.Brands.ToList();
                int brandsIndex = brands.FindIndex(c => c.BrandID == id) + 1;
                page = (int)Math.Ceiling((decimal)brandsIndex / 5);
            }
            PartialViewResult result = Modering("brands", page, "") as PartialViewResult;
            return PartialView(result.ViewName, result.Model);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult SaveDiscount(string Reason, decimal Size=0, int? id = 0)
        {
            Regex re = new Regex("[!@#$%^&*()_+`\\\"~[|/';<>{}=?№]");
            if ((db.Discounts.Count(p => p.Reason == Reason && p.Size == Size) > 0))
            {
                ViewBag.Error = "Данная скидка уже есть";
            }
            else if( Reason.Length < 3 || re.Match(Reason).Success || Size<0 || Size>50 || id < 0)
            {
                ViewBag.Error = "Ошибка сохранения, Ай-яй-яй";
            }
            else
            {
                if (id == 0)
                {
                    db.Discounts.Add(new Discount { Reason = Reason, Size = Size });
                }
                else
                {
                    var discount = db.Discounts.Where(p => p.DiscountID == id).FirstOrDefault();
                    if (discount == null)
                        ViewBag.Error = "Такой записи не существует";
                    else
                    {
                        discount.Reason = Reason;
                        discount.Size = Size;
                    }
                }
                db.SaveChanges();
            }
            int page = 1;
            if (id == 0)
                page = (int)Math.Ceiling((decimal)db.Discounts.Count() / 5);
            else
            {
                List<Discount> discounts = db.Discounts.ToList();
                int discountsIndex = discounts.FindIndex(c => c.DiscountID == id) + 1;
                page = (int)Math.Ceiling((decimal)discountsIndex / 5);
            }
            PartialViewResult result = Modering("discounts", page, "") as PartialViewResult;
            return PartialView(result.ViewName, result.Model);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult SaveCar(string CarNumber, int BrandID, int ColourID, DateTime ReleaseYear=new DateTime(), int? id=0)
        {
            Regex reg = new Regex(@"[авескмнортух]{1}[0-9]{3}[авескмнортух]{2}$");
            if ((db.Cars.Count(p => p.CarNumber == CarNumber && p.CarID != id) > 0))
            {
                ViewBag.Error = "Автомобиль с таким номером уже есть";
            }
            else if (ReleaseYear == new DateTime() || ReleaseYear >= DateTime.Now || id < 0
                || (db.Brands.Where(b => b.BrandID == BrandID).FirstOrDefault() == null) || (db.Colours.Where(c => c.ColourID == ColourID).FirstOrDefault() == null))
            {
                ViewBag.Error = "Ошибка сохранения, Ай-яй-яй";
            }
            else if (!reg.Match(CarNumber).Success)
            {
                ViewBag.Error = "Некорректный номер авто";
            }
            else
            {
                if (id == 0)
                {
                    db.Cars.Add(new Car { CarNumber = CarNumber, BrandID = BrandID, ColourID = ColourID, ReleaseYear = ReleaseYear });
                }
                else
                {
                    var car = db.Cars.Where(c => c.CarID == id).FirstOrDefault();
                    if (car == null)
                        ViewBag.Error = "Такой записи не существует";
                    else
                    {
                        car.CarNumber = CarNumber;
                        car.BrandID = BrandID;
                        car.ColourID = ColourID;
                        car.ReleaseYear = ReleaseYear;
                    }
                }
                db.SaveChanges();
            }
            int page = 1;
            if (id == 0)
                page = (int)Math.Ceiling((decimal)db.Cars.Count() / 5);
            else
            {
                List<Car> cars = db.Cars.ToList();
                int carsIndex = cars.FindIndex(c => c.CarID == id) + 1;
                page = (int)Math.Ceiling((decimal)carsIndex / 5);
            }
            PartialViewResult result = Modering("cars", page, "") as PartialViewResult;
            return PartialView(result.ViewName, result.Model);
        }
        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> SaveDriver(string FirstName, string Login, string Password, string LastName, string Patronymic, 
                                                   int CarID, int Experience = 0, DateTime BirthDate = new DateTime(), string Phone="")
        {
            Regex regexPhone = new Regex(@"[1-9]{3}-\d{3}-\d{2}-\d{2}$");
            Regex re = new Regex("[!@#$%^&*()_+`\\\"~[|/';:.,<>{}=?№ ]");
            if (re.Match(FirstName).Success || re.Match(LastName).Success || re.Match(Patronymic).Success ||
                FirstName.Length < 2 || LastName.Length < 2 ||
                (DateTime.Now - BirthDate).Days < 365 * 18 || BirthDate > DateTime.Now ||
                db.Cars.Where(c => c.CarID == CarID).FirstOrDefault() == null || Password.Length < 8 || Phone.Length < 13 )
            {
                ViewBag.Error = "Ошибка сохранения, поля заполнены неверно, Ай-яй-яй";
            }
            else if (db.Users.Where(u => u.UserName == Login).FirstOrDefault() != null)
            {
                ViewBag.Error = "Пользователь с таким логином уже существует";
            }
            else if (!regexPhone.Match(Phone).Success)
            {
                ViewBag.Error = "Вводите телефон в формате 123-456-78-90";
            }
            else
            {
                ApplicationUser user = new ApplicationUser { UserName = Login, Email = Login, DiscountID = 4, FIO = LastName + " " + FirstName + " " + Patronymic };
                IdentityResult userResult = await UserManager.CreateAsync(user, Password);
                if (userResult.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, "Driver");
                    UserManager.AddToRole(user.Id, "Driver");
                    db.Drivers.Add(new Driver { FirstName = FirstName, LastName = LastName, Patronymic = Patronymic, BirthDate = BirthDate, Experience = Experience, CarID = CarID, User = db.Users.Where(u => u.UserName == Login).FirstOrDefault() });
                    db.SaveChanges();
                }
            }
            int page = 1;
            page = (int)Math.Ceiling((decimal)db.Drivers.Count() / 5);
            PartialViewResult result = Modering("drivers", page, "") as PartialViewResult;
            return PartialView(result.ViewName, result.Model);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult SaveUserDiscount(string id, int DiscountID)
        {
            if (db.Discounts.Where(d => d.DiscountID == DiscountID).FirstOrDefault() == null || db.Users.Where(u=>u.Id==id).FirstOrDefault()==null)
            {
                ViewBag.Error = "Ошибка сохранения, Ай-яй-яй";
            }
            else
            {
                var user = db.Users.Where(u => u.Id == id).FirstOrDefault();
                if (user == null)
                    ViewBag.Error = "Такой записи не существует";
                else
                {
                    user.DiscountID = DiscountID;
                    db.SaveChanges();
                }
            }
            int page = 1;
            page = (int)Math.Ceiling((decimal)db.Users.Count() / 5);
            PartialViewResult result = Modering("users", page, "") as PartialViewResult;
            return PartialView(result.ViewName, result.Model);
        }
        #endregion
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Modering(string selected="", int page=1,string navig="")
        {
            int pageSize = 5;
            if (navig == "next")
            {
                page++;
            }
            else if (navig == "prev")
            {
                if (page != 1)
                    page--;
            }
            if (page < 1)
                page = 1;
            switch (selected)
            {
                case "brands":
                    {
                        IEnumerable<Brand> brandsPerPages = db.Brands.OrderBy(c => c.BrandID).ToList().Skip((page - 1) * pageSize).Take(pageSize);
                        PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = db.Brands.Count() };
                        if (brandsPerPages.Count() == 0)
                        {
                            page = pageInfo.TotalPages;
                            brandsPerPages = db.Brands.OrderBy(c => c.BrandID).ToList().Skip((page - 1) * pageSize).Take(pageSize);
                        }
                        ViewModel viewModel = new ViewModel { PageInfo = pageInfo, Brands = brandsPerPages };
                        ViewBag.NumberPage = page;
                        ViewBag.TotalPages = pageInfo.TotalPages;
                        return PartialView("BrandsPartial", viewModel);
                    }
                case "cars":
                    {
                        IEnumerable<Car> carsPerPages = db.Cars.Include(c => c.Brand).Include(c => c.Colour).OrderBy(c => c.CarID).ToList().Skip((page - 1) * pageSize).Take(pageSize);
                        PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = db.Cars.Count() };
                        if (carsPerPages.Count() == 0)
                        {
                            page = pageInfo.TotalPages;
                            carsPerPages = db.Cars.Include(c => c.Brand).Include(c => c.Colour).OrderBy(c => c.CarID).ToList().Skip((page - 1) * pageSize).Take(pageSize);
                        }
                        ViewModel viewModel = new ViewModel { PageInfo = pageInfo, Cars = carsPerPages };
                        ViewBag.NumberPage = page;
                        ViewBag.TotalPages = pageInfo.TotalPages;
                        return PartialView("CarsPartial", viewModel);
                        
                    }
                case "colours":
                    {
                        IEnumerable<Colour> coloursPerPages = db.Colours.OrderBy(c=>c.ColourID).ToList().Skip((page - 1) * pageSize).Take(pageSize);
                        PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = db.Colours.Count() };
                        if (coloursPerPages.Count()==0)
                        {
                            page = pageInfo.TotalPages;
                            coloursPerPages = db.Colours.OrderBy(c => c.ColourID).ToList().Skip((page - 1) * pageSize).Take(pageSize);
                        }
                        
                        ViewModel viewModel = new ViewModel { PageInfo = pageInfo, Colours = coloursPerPages };
                        ViewBag.NumberPage = page;
                        ViewBag.TotalPages = pageInfo.TotalPages;
                        return PartialView("ColoursPartial", viewModel);
                    }
                case "discounts":
                    {
                        IEnumerable<Discount> discountsPerPages = db.Discounts.OrderBy(c => c.DiscountID).ToList().Skip((page - 1) * pageSize).Take(pageSize);
                        PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = db.Discounts.Count() };
                        if (discountsPerPages.Count() == 0)
                        {
                            page = pageInfo.TotalPages;
                            discountsPerPages = db.Discounts.OrderBy(c => c.DiscountID).ToList().Skip((page - 1) * pageSize).Take(pageSize);
                        }
                        ViewModel viewModel = new ViewModel { PageInfo = pageInfo, Discounts = discountsPerPages };
                        ViewBag.NumberPage = page;
                        ViewBag.TotalPages = pageInfo.TotalPages;
                        return PartialView("DiscountsPartial", viewModel);
                    }
                case "drivers":
                    {
                        IEnumerable<Driver> driversPerPages = db.Drivers.Include(d => d.Car).OrderBy(d => d.DriverID).ToList().Skip((page - 1) * pageSize).Take(pageSize);
                        PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = db.Drivers.Count() };
                        if (driversPerPages.Count() == 0)
                        {
                            page = pageInfo.TotalPages;
                            driversPerPages = db.Drivers.Include(d => d.Car).OrderBy(d => d.DriverID).ToList().Skip((page - 1) * pageSize).Take(pageSize);
                        }
                        ViewModel viewModel = new ViewModel { PageInfo = pageInfo, Drivers = driversPerPages };
                        ViewBag.NumberPage = page;
                        ViewBag.TotalPages = pageInfo.TotalPages;
                        return PartialView("DriversPartial", viewModel);
                    }
                case "users":
                    {
                        IEnumerable<ApplicationUser> usersPerPages = db.Users.Include(u => u.Orders).Include(u => u.Roles).Include(u => u.Discount).OrderBy(u => u.Id).ToList().Skip((page - 1) * pageSize).Take(pageSize);
                        PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = db.Users.Count() };
                        if (usersPerPages.Count() == 0)
                        {
                            page = pageInfo.TotalPages;
                            usersPerPages = db.Users.Include(u => u.Orders).Include(u => u.Roles).Include(u => u.Discount).OrderBy(u => u.Id).ToList().Skip((page - 1) * pageSize).Take(pageSize);
                        }
                        ViewModel viewModel = new ViewModel { PageInfo = pageInfo, Users = usersPerPages };
                        ViewBag.NumberPage = page;
                        ViewBag.TotalPages = pageInfo.TotalPages;
                        return PartialView("UsersPartial", viewModel);
                    }
                case "orders":
                    {
                        IEnumerable<Order> ordersPerPages = CONVERTER3000.TRANSFORMATION(
                            db.Orders
                            .Include(o => o.ApplicationUser)
                            .Include(o => o.Driver)
                            .Include(o => o.Driver.Car)
                            .OrderBy(o => o.OrderID)
                            .ToList()
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToList());
                        PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = db.Orders.Count() };
                        if (ordersPerPages.Count() == 0)
                        {
                            page = pageInfo.TotalPages;
                            ordersPerPages = CONVERTER3000.TRANSFORMATION(db.Orders.Include(o => o.ApplicationUser).Include(o => o.Driver)
                                .Include(o => o.Driver.Car)
                                .OrderBy(o => o.OrderID).ToList().Skip((page - 1) * pageSize).Take(pageSize).ToList());
                        }
                        List<State> states = new List<State>();
                        foreach (Order order in ordersPerPages)
                        {
                            State state = db.OrderStates.Where(o => o.OrderID == order.OrderID).OrderBy(o => o.date).ToList().Last().state;
                            states.Add(state);
                        }
                        ViewModel viewModel = new ViewModel { PageInfo = pageInfo, Orders = ordersPerPages, States = states };
                        ViewBag.NumberPage = page;
                        ViewBag.TotalPages = pageInfo.TotalPages;
                        return PartialView("OrdersPartial", viewModel);
                    }
                default:
                    {
                        PartialViewResult result = Modering("colours", 1, "") as PartialViewResult;
                        return PartialView(result.ViewName,result.Model);
                    }
            }
        }
    }
}
