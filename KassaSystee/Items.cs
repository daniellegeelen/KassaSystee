using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KassaSystee
{
    public class Items
    {
        public string Code = "";
        public string Description = "";
        public double Price = 0.0;
        public Items(string sCode, string sDescription, double dPrice)
        {
            Price = dPrice;
            Code = sCode;
            Description = sDescription;
        }
        public Items() { }
    }
}
