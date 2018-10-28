using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Курсач.Models
{
    public class State
    {
        public int StateID { get; set; }
        public string StateName { get; set; }

        public virtual ICollection<OrderState> Orders { get; set; }
    }
}