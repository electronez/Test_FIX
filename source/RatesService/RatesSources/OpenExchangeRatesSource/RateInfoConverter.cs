namespace OpenExchangeRatesSource
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Rates.Common;

    public static class RateInfoConverter
    {
        public static IEnumerable<RateInfo> Convert(OpenExchangeRateEntity rateEntity)
        {
            var date = DateTimeOffset.FromUnixTimeSeconds(rateEntity.Timestamp).DateTime.ToLocalTime();
            return rateEntity.Rates.Select(o => new RateInfo() {To = o.Key, Rate = o.Value});
        }
    }
}