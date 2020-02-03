namespace OpenExchangeRatesSource
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Rates.Common;

    /// <summary>
    /// Конвертер сущности <see cref="OpenExchangeRateEntity"/> в <see cref="IEnumerable{RateInfo}"/>
    /// </summary>
    public static class RateInfoConverter
    {
        /// <summary>
        /// Конвертировать <see cref="OpenExchangeRateEntity"/> в <see cref="IEnumerable{RateInfo}"/>
        /// </summary>
        /// <param name="rateEntity">Сущность с курсами валют из источника</param>
        /// <returns>Колекция курсов валют</returns>
        public static IEnumerable<RateInfo> Convert(OpenExchangeRateEntity rateEntity)
        {
            var date = DateTimeOffset.FromUnixTimeSeconds(rateEntity.Timestamp).DateTime.ToLocalTime();
            return rateEntity.Rates.Select(o => new RateInfo() {To = o.Key, Rate = o.Value});
        }
    }
}