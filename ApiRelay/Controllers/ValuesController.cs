using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiLibrary;
using ApiLibrary.DataModel;
using Microsoft.AspNetCore.Mvc;

namespace ApiRelay.Controllers
{
    [Route("api/CurrencyRates")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/CurrencyRates
        [HttpGet("{dateFrom}/{dateTo}/{table}/{currency}")]
        public async Task<IEnumerable<CurrencyRoot>> Get(DateTime dateFrom, DateTime dateTo, string currency, string table)
        {
            var currencyRoot = await GetFromNBPApi.CurrencyRate(dateFrom, dateTo, currency, table);
            return currencyRoot;
        }
    }
}
