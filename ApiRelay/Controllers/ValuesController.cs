using System;
using System.Collections.Generic;
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
        [HttpGet("{dateFrom}/{dateTo}/{currency}/{table}")]
        public async Task<IEnumerable<RootObject>> Get(DateTime dateFrom, DateTime dateTo, string currency, string table)
        {
            return await GetFromNBPApi.CurrencyRate(dateFrom, dateTo, new Rate{ code = currency, Table = table});
        }
    }
}
