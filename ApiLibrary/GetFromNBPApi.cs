using ApiLibrary.DataModel;
using Flurl;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiLibrary
{

    public class GetFromNBPApi
    {
        private readonly static string ApiUrl = @"http://api.nbp.pl/api/";
        private readonly static int ApiDayLimit = 93;
        private readonly static int ApiGoldPriceDayLimit = 367;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets available curremcies
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<Rate>> Symbols()
        {
            var tables = new List<string> { "A", "B" };
            var results = new List<Rate>();

            foreach (var table in tables)
            {
                var url = Url.Combine(ApiUrl,
                                      "exchangerates/tables",
                                      table);

                try
                {
                    using (var client = new HttpClient())
                    {
                        var currencyJson = await client.GetStringAsync(url);
                        var symbols = JsonConvert.DeserializeObject<List<RootObject>>(currencyJson);
                        var rates = from s in symbols[0].rates
                                    select new Rate { code = s.code, currency = s.currency, mid = s.mid, Table = table };
                        results.AddRange(rates);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return results;
        }

        /// <summary>
        /// Gets currency rate between given dates
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<CurrencyRoot>> CurrencyRate(DateTime dateFrom, DateTime dateTo, Rate currency)
        {
            var root = new List<CurrencyRoot>();
            var dates = ConformDates(dateFrom, dateTo, ApiDayLimit);

            foreach (var date in dates)
            {
                var url = Url.Combine(ApiUrl,
                                      "exchangerates/rates",
                                      currency.Table,
                                      currency.code,
                                      date.Item1.ToString("yyyy-MM-dd"),
                                      date.Item2.ToString("yyyy-MM-dd"));

                try
                {
                    using (var client = new HttpClient())
                    {
                        var currencyJson = await client.GetStringAsync(url);
                        root.Add(JsonConvert.DeserializeObject<CurrencyRoot>(currencyJson));
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }

            return root;
        }

        public static async Task<IEnumerable<CurrencyRoot>> CurrencyRate(DateTime dateFrom, DateTime dateTo, string currency, string table)
        {
            var root = new List<CurrencyRoot>();
            var dates = ConformDates(dateFrom, dateTo, ApiDayLimit);

            foreach (var date in dates)
            {
                var url = Url.Combine(ApiUrl,
                                      "exchangerates/rates",
                                      table,
                                      currency,
                                      date.Item1.ToString("yyyy-MM-dd"),
                                      date.Item2.ToString("yyyy-MM-dd"));

                try
                {
                    using (var client = new HttpClient())
                    {
                        var currencyJson = await client.GetStringAsync(url);
                        root.Add(JsonConvert.DeserializeObject<CurrencyRoot>(currencyJson));
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }

            return root;
        }

        private static List<Tuple<DateTime, DateTime>> ConformDates(DateTime dateFrom, DateTime dateTo, int dayLimit)
        {
            var dates = new List<Tuple<DateTime, DateTime>>();

            if ((dateFrom - dateTo).TotalDays < -dayLimit)
            {
                while ((dateFrom - dateTo).TotalDays < -dayLimit)
                {
                    dates.Add(new Tuple<DateTime, DateTime>(dateFrom, dateFrom.AddDays(dayLimit)));

                    dateFrom = dateFrom.AddDays(dayLimit + 1);
                }

                dates.Add(new Tuple<DateTime, DateTime>(dateFrom, dateTo));
            }
            else
            {
                dates.Add(new Tuple<DateTime, DateTime>(dateFrom, dateTo));
            }

            return dates;
        }

        /// <summary>
        /// Gets gold prices between given dates
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<GoldPrice>> GoldPrice(DateTime dateFrom, DateTime dateTo)
        {
            var dates = ConformDates(dateFrom, dateTo, ApiGoldPriceDayLimit);
            var goldPrices = new List<GoldPrice>();

            foreach (var date in dates)
            {
                var url = Url.Combine(ApiUrl,
                                      "cenyzlota",
                                      date.Item1.ToString("yyyy-MM-dd"),
                                      date.Item2.ToString("yyyy-MM-dd"));

                try
                {
                    using (var client = new HttpClient())
                    {
                        var currencyJson = await client.GetStringAsync(url);
                        goldPrices.AddRange(JsonConvert.DeserializeObject<List<GoldPrice>>(currencyJson));
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }

            return goldPrices;
        }

        public static async Task<IEnumerable<CurrencyRoot>> SalePurchaseRate(DateTime dateFrom, DateTime dateTo, Rate currency)
        {
            var root = new List<CurrencyRoot>();
            var dates = ConformDates(dateFrom, dateTo, ApiDayLimit);

            foreach (var date in dates)
            {
                var url = Url.Combine(ApiUrl,
                                      "exchangerates/rates",
                                      "C",
                                      currency.code,
                                      date.Item1.ToString("yyyy-MM-dd"),
                                      date.Item2.ToString("yyyy-MM-dd"));

                try
                {
                    using (var client = new HttpClient())
                    {
                        var currencyJson = await client.GetStringAsync(url);
                        root.Add(JsonConvert.DeserializeObject<CurrencyRoot>(currencyJson));
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }

            return root;
        }
    }
}
