using CurrencyService.Models;

namespace CurrencyService.Interfaces
{
    /// <summary>
    /// Інтерфейс Сервісу для отримання даних про валюту
    /// </summary>
    public interface ICurrencyService
    {
        /// <summary>
        /// Отримання списку валют 
        /// </summary>
        /// <param name="exchangeDate">Дата обміну</param>
        /// <returns></returns>
        public Task<List<Currency>> GetCurrencyListAsync(DateTime exchangeDate);

        /// <summary>
        /// Отримання валюти
        /// </summary>
        /// <param name="currencyCode">Код валюти</param>
        /// <param name="exchangeDate">Дата обміну</param>
        /// <returns></returns>
        public Task<Currency?> GetCurrencyAsync(int currencyCode, DateTime exchangeDate);
    }
}
