using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibrary.DataModel
{
    public class Rate
    {
        public string currency { get; set; }
        public string code { get; set; }
        public double mid { get; set; }

        public string Table { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Rate rate &&
                   currency == rate.currency &&
                   code == rate.code;
        }

        public override string ToString()
        {
            return currency + " - " + code;
        }
    }
}
