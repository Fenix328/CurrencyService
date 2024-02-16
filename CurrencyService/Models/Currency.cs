using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace CurrencyService.Models
{
    /// <summary>
    /// Модель валюти
    /// </summary>
    public class Currency
    {
        /// <summary>
        /// Код валюти по r030
        /// </summary>
        [JsonProperty("r030")]
        public int Code {  get; set; }

        /// <summary>
        /// Текстовий код валюти
        /// </summary>
        [JsonProperty("cc")]
        public string Key { get; set; } = string.Empty;

        /// <summary>
        /// Назва 
        /// </summary>
        [JsonProperty("txt")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Значення
        /// </summary>
        [JsonProperty("rate")]
        public decimal Value { get; set; }

        /// <summary>
        /// Дата 
        /// </summary>
        [JsonProperty("exchangedate")]
        public string ExchangeDateStr { get; set; } = string.Empty;

    }
}
