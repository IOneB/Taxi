using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Курсач.Models
{
    public class Colour
    {
        public int ColourID { get; set; }
        //Наименование цвета
        [Required]
        [StringLength(50,MinimumLength =3)]
        public string ColourName { get; set; }
        //Автомобили данного цвета
        public List<Car> Cars { get; set; }
    }
}