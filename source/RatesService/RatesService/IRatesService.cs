namespace RatesService
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Rates.Common;

    /// <summary>
    /// Интерфейс сервиса для получения курсов валют
    /// </summary>
    public interface IRatesService
    {
        /// <summary>
        /// Получение курсов валют
        /// </summary>
        /// <param name="from">Продаваемая валюта</param>
        /// <param name="to">Покупаемая валюта</param>
        /// <returns>Коллекция курсов валют</returns>
        Task<IEnumerable<ExpiredRateInfo>> GetRates(string from, string to = null);
    }
}