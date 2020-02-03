namespace RatesSources.Common
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Rates.Common;

    public interface IRatesSourceProvider
    {
        Task<IEnumerable<RateInfo>> GetRatesAsync(string from);
    }
}