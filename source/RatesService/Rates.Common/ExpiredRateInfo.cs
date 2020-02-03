namespace Rates.Common
{
    using System;

    /// <summary>
    /// Информация о запрошенном курсе с указанием даты истечения срока действия
    /// </summary>
    public class ExpiredRateInfo : RateInfo
    {
        /// <summary>
        /// Время истечения курса
        /// </summary>
        public DateTime ExpireAt { get; set; }
    }
}