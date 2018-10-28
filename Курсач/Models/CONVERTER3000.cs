using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Курсач.Models
{
    static public class CONVERTER3000
    {
        static public List<Order> TRANSFORMATION(List<Order> list)
        {
            List<Order> newList = new List<Order>();
            newList = list.GetRange(0, list.Count);
            foreach(var element in newList)
            {
                element.StartGPS = element.StartGPS.Replace(" Хабаровск, Хабаровский край, Россия,", "");
                element.EndGPS = element.EndGPS.Replace(" Хабаровск, Хабаровский край, Россия,", "");
                element.IntermediateGPS1 = element.IntermediateGPS1.Replace(" Хабаровск, Хабаровский край, Россия,", "");
                element.IntermediateGPS2 = element.IntermediateGPS2.Replace(" Хабаровск, Хабаровский край, Россия,", "");
                element.IntermediateGPS3 = element.IntermediateGPS3.Replace(" Хабаровск, Хабаровский край, Россия,", "");
            }
            return newList;
        }
    }
}