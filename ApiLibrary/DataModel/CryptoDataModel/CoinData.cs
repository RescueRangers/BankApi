using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibrary.DataModel.CryptoDataModel
{
    public class CoinData
    {
        public Coin CoinInfo { get; set; }
        public RAW RAW { get; set; }
        public DISPLAY DISPLAY { get; set; }
    }
}
