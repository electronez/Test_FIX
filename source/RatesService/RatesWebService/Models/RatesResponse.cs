namespace RatesWebService.Models
{
    using System.ComponentModel.DataAnnotations;
    using Rates.Common;

    /// <summary>
    /// Ответ на запрос курсов
    /// </summary>
    public class RatesResponse
    {
        /// <summary>
        /// Отдаваемая валюта
        /// </summary>
        [MinLength(3), MaxLength(3)]
        public string From { get; set; }

        /// <summary>
        /// Данные по запрошенным курсам
        /// </summary>
        public RateInfo[] Rates { get; set; }
    }
}
