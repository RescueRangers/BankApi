using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibrary.DataModel.CryptoDataModel
{
    public class CryptoResponse
    {
        public string Response { get; set; }
        public int Type { get; set; }
        public bool Aggregated { get; set; }
        public List<Datum> Data { get; set; }
        public int TimeTo { get; set; }
        public int TimeFrom { get; set; }
        public bool FirstValueInArray { get; set; }
        public ConversionType ConversionType { get; set; }
        public RateLimit RateLimit { get; set; }
        public bool HasWarning { get; set; }
        public string Message { get; set; }
        public string BaseImageUrl { get; set; }
        public string BaseLinkUrl { get; set; }
    }
}
