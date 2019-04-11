using ApiLibrary.DataModel.CryptoDataModel;
using Flurl;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiLibrary
{
    public class GetFromCryptoApi
    {
        private const string CryptoApi = "https://min-api.cryptocompare.com";
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string ApiKey = "9518f9b801e8f05469a75e055e524c3cc03b0a6b24dcfd66de11e38edf49d37f";

        public static async Task<CryptoResponse> HistoricalDaily(Coin coin)
        {
            var url = Url.Combine(CryptoApi, "data/histoday").SetQueryParams(new { fsym = coin.Name, tsym = "PLN", limit = 31, api_key = ApiKey });

            try
            {
                using (var client = new HttpClient())
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var currencyJson = await client.GetStringAsync(url);
                    return JsonConvert.DeserializeObject<CryptoResponse>(currencyJson);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return new CryptoResponse { Response = "Error", HasWarning = true, Message = ex.Message};
            }
        }

        public static async Task<CoinResponse> Coins()
        {
            var url = Url.Combine(CryptoApi, "data/top/totalvolfull").SetQueryParams(new {limit = 100, tsym = "PLN", api_key = ApiKey });

            try
            {
                using (var client = new HttpClient())
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var currencyJson = await client.GetStringAsync(url);
                    return JsonConvert.DeserializeObject<CoinResponse>(currencyJson);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return new CoinResponse { HasWarning = true, Message = ex.Message };
            }
        }
    }
}
