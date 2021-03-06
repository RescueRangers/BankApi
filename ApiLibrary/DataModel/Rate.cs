﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibrary.DataModel
{
    public class Rate : IEquatable<Rate>
    {
        public string currency { get; set; }
        public string code { get; set; }
        public double mid { get; set; }
        public string no { get; set; }
        public string effectiveDate { get; set; }
        public double bid { get; set; }
        public double ask { get; set; }

        public string Table { get; set; }

        public override string ToString()
        {
            return currency + " - " + code;
        }

        public bool Equals(Rate other)
        {
            return currency == other.currency &&
                   code == other.code;
        }
    }
}
