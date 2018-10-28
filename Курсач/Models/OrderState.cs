using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Курсач.Models
{
    public class OrderState
    {
        [Key, Column(Order = 1)]
        public int OrderID { get; set; }
        [Key, Column(Order = 2)]
        public int StateID { get; set; }

        public virtual Order order { get; set; }
        public virtual State state { get; set; }
        [Key, Column(Order = 0)]
        public DateTime date { get; set; }
        public string user { get; set; }
    }
}