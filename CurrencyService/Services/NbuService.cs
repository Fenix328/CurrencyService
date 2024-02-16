using CurrencyService.Interfaces;
using CurrencyService.Models;
using Newtonsoft.Json;

namespace CurrencyService.Services
{
    /// <summary>
    /// Сервіс для отримання даних про валюту
    /// </summary>
    public class NbuService : ICurrencyService
    {
        private readonly string _bankUrl;
        private readonly HttpClient _httpClient;
        private readonly ILogger<NbuService> _logger;

        private string GetCurrencyListUrl(DateTime exchangeDate) => $"{_bankUrl}/NBUStatService/v1/statdirectory/exchange?date={exchangeDate:yyyyMMdd}&json";

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="httpClientFactory"></param>
        /// <param name="logger"></param>
        public NbuService(IConfiguration configuration, IHttpClientFactory httpClientFactory, ILogger<NbuService> logger)
        {
            _bankUrl = configuration.GetValue<string>("BankGovUrl");
            _httpClient = httpClientFactory.CreateClient("Client");
            _logger = logger;

        }

        /// <summary>
        /// Отримання валюти
        /// </summary>
        /// <param name="currencyCode">Код валюти</param>
        /// <param name="exchangeDate">Дата обміну</param>
        /// <returns></returns>
        public async Task<Currency?> GetCurrencyAsync(int currencyCode, DateTime exchangeDate)
        {
            List<Currency> currencies = await GetCurrencyListAsync(exchangeDate);

            Currency? currency = currencies.FirstOrDefault(c => c.Code == currencyCode);

            return currency;
        }

        /// <summary>
        /// Отримання списку валют 
        /// </summary>
        /// <param name="exchangeDate">Дата обміну</param>
        /// <returns></returns>
        public async Task<List<Currency>> GetCurrencyListAsync(DateTime exchangeDate)
        {
            List<Currency> result = new();

            try
            {
                var content = await GetContent(GetCurrencyListUrl(exchangeDate));

                if (string.IsNullOrEmpty(content))
                    throw new Exception("content is null or empty");

                result = JsonConvert.DeserializeObject<List<Currency>>(content);
            }
            catch (Exception ex)
            {
                _logger.LogError("GetCurrencyList: {exchangeDate}, Error: {message}", exchangeDate, ex.Message);
            }

            return result;
        }

        /// <summary>
        /// Отримання даних з вказаної URL
        /// </summary>
        /// <param name="url">Url звідки хочемо отримати дані</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private async Task<string> GetContent(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException(nameof(url));

            return await _httpClient.GetStringAsync(url);
        }
    }
}
