namespace Rates.Common
{
    using System;

    public class ExpiredRateInfo : RateInfo
    {
        /// <summary>
        /// Время истечения курса
        /// </summary>
        public DateTime ExpireAt { get; set; }
    }
}