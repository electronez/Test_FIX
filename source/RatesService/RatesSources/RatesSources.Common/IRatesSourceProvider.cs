namespace RatesSources.Common
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Rates.Common;

    /// <summary>
    /// Интерфейс провайдера получения курсов валют
    /// </summary>
    public interface IRatesSourceProvider
    {
        /// <summary>
        /// Получение курсов валют для указанной валюты
        /// </summary>
        /// <param name="from">Валюта</param>
        /// <returns>Коллекция курсов валют</returns>
        Task<IEnumerable<RateInfo>> GetRatesAsync(string from);
    }
}