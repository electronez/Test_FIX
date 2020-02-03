namespace RatesStorageService
{
    using System.Collections.Generic;
    using Rates.Common;

    public interface IRatesStorageService
    {
        void SaveRates(string from, IEnumerable<ExpiredRateInfo> rates);

        IEnumerable<ExpiredRateInfo> GetRates(string from);

        IEnumerable<ExpiredRateInfo> GetRates(string from, string to);
    }
}