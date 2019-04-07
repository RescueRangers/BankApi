using System.Collections.Generic;

namespace ApiLibrary.DataModel
{
    public class CurrencyRoot
    {
        public string table { get; set; }
        public string currency { get; set; }
        public string code { get; set; }
        public List<CurrencyRate> rates { get; set; }
    }
}
