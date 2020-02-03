namespace RatesStorageService
{
    using System.Collections.Generic;
    using Rates.Common;

    /// <summary>
    /// Интерфейс сервиса получения/сохранения данных в БД
    /// </summary>
    public interface IRatesStorageService
    {
        /// <summary>
        /// Сохранение курсов валют в БД
        /// </summary>
        /// <param name="from">Продаваемая валюта</param>
        /// <param name="rates">Коллекция курсов валют</param>
        void SaveRates(string from, IEnumerable<ExpiredRateInfo> rates);

        /// <summary>
        /// Получение курсов валют из БД
        /// </summary>
        /// <param name="from">Продаваемая валюта</param>
        /// <returns>Коллекция курсов валют</returns>
        IEnumerable<ExpiredRateInfo> GetRates(string from);

        /// <summary>
        /// Получение курсов валют из БД
        /// </summary>
        /// <param name="from">Продаваемая валюта</param>
        /// <param name="to">Покупаемая валюта</param>
        /// <returns>Коллекция курсов валют</returns>
        IEnumerable<ExpiredRateInfo> GetRates(string from, string to);
    }
}