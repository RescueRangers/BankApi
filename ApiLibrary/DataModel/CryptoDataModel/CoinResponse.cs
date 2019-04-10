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
        public string Response { get; set; }
        public string Message { get; set; }
        [JsonProperty("Data")]
        public Dictionary<string, Coin> Data { get; set; }
        public string BaseImageUrl { get; set; }
        public string BaseLinkUrl { get; set; }
        public RateLimit RateLimit { get; set; }
        public bool HasWarning { get; set; }
        public int Type { get; set; }
    }
}
