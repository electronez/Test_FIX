namespace OpenExchangeRatesSource
{
    using System.Collections.Generic;

    public class OpenExchangeRateEntity
    {
        public string Disclaimer { get; set; }

        public string License { get; set; }

        public int Timestamp { get; set; }

        public string Base { get; set; }

        public Dictionary<string, decimal> Rates { get; set; }
    }
}