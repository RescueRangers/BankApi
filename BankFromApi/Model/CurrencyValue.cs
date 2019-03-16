﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFromApi.Model
{
    public class CurrencyValue
    {
        public DateTime Date { get; set; }
        public double Value { get; set; }

        public CurrencyValue(DateTime date, double value)
        {
            Date = date;
            Value = value;
        }
    }
}
