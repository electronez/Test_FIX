namespace OpenExchangeRatesSource
{
    using System.Net.Http;
    using RatesSources.Common;
    using System;
    using Newtonsoft.Json;
    using System.Threading.Tasks;
    using Rates.Common;
    using System.Collections.Generic;

    public class OpenExchangeSourseProvider : IRatesSourceProvider
    {
        public static readonly Uri BaseUrl = new Uri("https://openexchangerates.org/api/");

        private readonly IHttpClientFactory httpClientFactory;
        private readonly string appId;

        public OpenExchangeSourseProvider(IHttpClientFactory httpClientFactory, string appId)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.appId = !string.IsNullOrEmpty(appId) ? appId : throw new ArgumentNullException(nameof(appId));
        }

        public async Task<IEnumerable<RateInfo>> GetRatesAsync(string from)
        {
            var httpClient = this.httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Token {this.appId}");
            var ratesUri = new Uri(OpenExchangeSourseProvider.BaseUrl, $"latest.json?base={from}");
            var response = await httpClient.GetAsync(ratesUri);
            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException(message);
            }

            var ratesResult = await response.Content.ReadAsStringAsync();
            var rates = JsonConvert.DeserializeObject<OpenExchangeRateEntity>(ratesResult);
            return RateInfoConverter.Convert(rates);
        }
    }
}