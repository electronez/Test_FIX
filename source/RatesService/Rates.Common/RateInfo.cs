namespace Rates.Common
{
    /// <summary>
    /// Информация о запрошенном курсе
    /// </summary>
    public class RateInfo
    {
        /// <summary>
        /// Покупаемая валюта
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Значение курса
        /// </summary>
        public decimal Rate { get; set; }
    }
}
