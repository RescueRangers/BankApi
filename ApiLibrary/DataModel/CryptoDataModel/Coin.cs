using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibrary.DataModel.CryptoDataModel
{
    public class Coin
    {
        private const string BaseUrl = "https://www.cryptocompare.com";

        public string Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Internal { get; set; }
        public string ImageUrl { get; set; }
        public string Url { get; set; }
        public string Algorithm { get; set; }
        public string ProofType { get; set; }
        public double NetHashesPerSecond { get; set; }
        public int BlockNumber { get; set; }
        public int BlockTime { get; set; }
        public double BlockReward { get; set; }
        public int Type { get; set; }
        public string DocumentType { get; set; }

        public string FullImageUrl => BaseUrl + ImageUrl;

        public override string ToString()
        {
            return string.Format("{0} - ({1})", FullName, Name);
        }
    }
}
