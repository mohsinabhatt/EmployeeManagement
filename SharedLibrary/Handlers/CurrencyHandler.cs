using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;

namespace SharedLibrary
{
    public static class CurrencyHandler
    {
        private static Dictionary<Currency, decimal> CurrencyRates;
        private static DateTime UpdatedOn;
        readonly static object _currencyLock = new object();
        internal static readonly Dictionary<Currency, string> CurrencyCultureMap;

        static CurrencyHandler()
        {
            CurrencyCultureMap = new Dictionary<Currency, string>() {
                { Currency.INR, "en-IN"},
                { Currency.USD, "en-US"},
                { Currency.EUR, "en-ES"},
                { Currency.GBP, "en-GB"}
            };
        }

        /// <summary>
        /// Gets the currency rates from base USD
        /// </summary>
        /// <param name="currency">The currency code you want to get the rate of.</param>
        /// <returns>Returns the currency rate against base USD</returns>
        public static decimal GetCurrencyRate(Currency currency)
        {
            var isExpired = CurrencyRates.IsNullOrEmpty() || UpdatedOn.AddHours(12) <= DateTime.UtcNow;
            if (isExpired)
            {
                lock (_currencyLock)
                {
                    if (isExpired)
                    {
                        using (var client = new HttpClient())
                        {
                            var response = client.GetAsync("https://openexchangerates.org/api/latest.json?app_id=35419b2b9c9f4fa897a708440622611e&symbols=GBP,EUR,AED,CAD,INR").Result;
                            var stringContent = response.Content.ReadAsStringAsync().Result;
                            if (response.IsSuccessStatusCode)
                            {
                                var rates = JsonConvert.DeserializeObject<CurrencyRate>(stringContent);
                                UpdatedOn = DateTime.UtcNow;
                                CurrencyRates = rates.Rates;
                                CurrencyRates.TryAdd(Currency.USD, 1);
                            }
                            else
                                throw new Exception(stringContent);
                        }
                    }
                }
            }

            return CurrencyRates[currency];
        }


        public static decimal ConvertCurrency(this decimal value, Currency currency)
        {
            return decimal.Round(value * GetCurrencyRate(currency), 2, MidpointRounding.AwayFromZero);
        }


        public static string ConvertCurrencyWithSymbol(this decimal value, Currency currency)
        {
            return decimal.Round(value.ConvertCurrency(currency), 2, MidpointRounding.AwayFromZero).ToString("C", new CultureInfo(CurrencyCultureMap[currency]));
        }


        public static string ToCurrencyString(this decimal value, Currency currency)
        {
            return value.ToString("C", new CultureInfo(CurrencyCultureMap[currency]));
        }


        private sealed class CurrencyRate
        {
            public Dictionary<Currency, decimal> Rates { get; set; }
        }
    }
}
