using CurrencyService.Interfaces;
using CurrencyService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyService.Controllers
{
    /// <summary>
    /// Отримання даних про валюту
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="currencyService"></param>
        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        /// <summary>
        /// Дані про конкретну валюту
        /// </summary>
        /// <param name="currencyCode">Код валюти</param>
        /// <param name="exchangeDate">Дата обміну</param>
        /// <returns></returns>
        [HttpGet("Get")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Currency))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCurrency(int currencyCode, DateTime? exchangeDate)
        {
            if (!exchangeDate.HasValue)
                exchangeDate = DateTime.Now;

            var currency = await _currencyService.GetCurrencyAsync(currencyCode, exchangeDate.Value);

            if (currency == null)
                return NotFound($"Валюту з кодом {currencyCode} не знайдено!");

            return Ok(currency);
        }

        /// <summary>
        /// Отримання списку валют
        /// </summary>
        /// <param name="exchangeDate">Дата обміну</param>
        /// <returns></returns>
        [HttpGet("GetList")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Currency>))]
        public async Task<IActionResult> GetCurrencyList(DateTime? exchangeDate)
        {
            if (!exchangeDate.HasValue)
                exchangeDate = DateTime.Now;

            var currencyList = await _currencyService.GetCurrencyListAsync(exchangeDate.Value);

            return Ok(currencyList);
        }
    }
}
