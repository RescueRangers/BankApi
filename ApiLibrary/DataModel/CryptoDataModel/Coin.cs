using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibrary.DataModel.CryptoDataModel
{
    public class Coin
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string CoinName { get; set; }
        public string FullName { get; set; }
        public string Algorithm { get; set; }
        public string ProofType { get; set; }
        public string FullyPremined { get; set; }
        public string TotalCoinSupply { get; set; }
        public string BuiltOn { get; set; }
        public string SmartContractAddress { get; set; }
        public string PreMinedValue { get; set; }
        public string TotalCoinsFreeFloat { get; set; }
        public string SortOrder { get; set; }
        public bool Sponsored { get; set; }
        public bool IsTrading { get; set; }
        public int TotalCoinsMined { get; set; }
        public int BlockNumber { get; set; }
        public int NetHashesPerSecond { get; set; }
        public int BlockReward { get; set; }
        public int BlockTime { get; set; }
    }
}
