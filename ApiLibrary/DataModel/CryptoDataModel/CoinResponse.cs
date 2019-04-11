using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibrary.DataModel.CryptoDataModel
{
    public class CoinResponse
    {
        public string Message { get; set; }
        public int Type { get; set; }
        public IList<object> SponsoredData { get; set; }
        public IList<CoinData> Data { get; set; }
        public RateLimit RateLimit { get; set; }
        public bool HasWarning { get; set; }
    }
}
