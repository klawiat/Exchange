using Exchange.Api.Models;
using Exchange.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Exchange.Api.Controllers
{
    public class ExchangeController : Controller
    {
        private readonly IMemoryCache cache;
        private readonly ExchangeRateService service;

        public ExchangeController(IMemoryCache cache, ExchangeRateService service)
        {
            this.cache = cache;
            this.service = service;
        }
        [HttpGet("[action]")]
        public IActionResult GetExchangeRate([FromQuery] DateTime? date)
        {
            string key = date.HasValue ? date.Value.ToShortDateString() : DateTime.UtcNow.ToShortDateString();
            List<Valute> valutes;
            if (!cache.TryGetValue(key, out valutes))
            {
                valutes = service.GetQuotes(date);
                cache.Set(key, valutes);
            }
            return Json(valutes);
        }
        [HttpGet("[action]/{from}/{to}/{amount?}")]
        public IActionResult Convert([FromQuery] DateTime? date, [FromRoute] string from, [FromRoute] string to, [FromRoute] decimal amount = 1)
        {
            List<Valute> valutes = GetQuotes(date);
            Models.ViewModels.ConvertedCurrenciesVM result = service.ConvertValute(from, to, valutes, amount);
            return Json(result);
        }
        [NonAction]
        private List<Valute> GetQuotes(DateTime? date)
        {
            string key = date.HasValue ? date.Value.ToShortDateString() : DateTime.UtcNow.ToShortDateString();
            List<Valute> valutes;
            if (!cache.TryGetValue(key, out valutes))
            {
                valutes = service.GetQuotes(date);
                cache.Set(key, valutes);
            }
            return valutes;
        }
    }
}
