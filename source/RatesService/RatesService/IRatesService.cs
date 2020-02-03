namespace RatesService
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Rates.Common;

    public interface IRatesService
    {
        Task<IEnumerable<ExpiredRateInfo>> GetRates(string from, string to = null);
    }
}