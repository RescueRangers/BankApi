using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibrary.DataModel
{
    public class RootObject
    {
        public string table { get; set; }
        public string no { get; set; }
        public string tradingDate { get; set; }
        public string effectiveDate { get; set; }
        public List<Rate> rates { get; set; }
        public string currency { get; set; }
        public string code { get; set; }
    }
}
