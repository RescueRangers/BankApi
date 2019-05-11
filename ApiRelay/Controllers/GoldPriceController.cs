using ApiLibrary;
using ApiLibrary.DataModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiRelay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoldPriceController : ControllerBase
    {
        [HttpGet("{dateFrom}/{dateTo}")]
        public async Task<IEnumerable<GoldPrice>> Get(DateTime dateFrom, DateTime dateTo)
        {
            var goldPrices = await GetFromNBPApi.GoldPrice(dateFrom, dateTo);

            return goldPrices;
        }
    }
}
