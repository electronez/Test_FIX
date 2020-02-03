namespace RatesService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Rates.Common;
    using RatesSources.Common;
    using RatesStorageService;

    /// <summary>
    /// Сервис для получения курсов валют
    /// </summary>
    public class RatesService : IRatesService
    {
        private readonly IRatesSourceProvider ratesSourceProvider;
        private readonly IRatesStorageService ratesStorageService;
        private readonly int invinvalidateMinutes;

        /// <summary>
        /// Инициализирует экземпляр класса <see cref="RatesService"/>
        /// </summary>
        /// <param name="ratesSourceProvider">Провайдер источника данных о курсах валют</param>
        /// <param name="ratesStorageService">Сервис хранения данных о курсах валют</param>
        /// <param name="invalidateMinutes">Время актуальности курсов валют</param>
        public RatesService(IRatesSourceProvider ratesSourceProvider, IRatesStorageService ratesStorageService, int invalidateMinutes)
        {
            this.ratesSourceProvider = ratesSourceProvider ?? throw new ArgumentNullException(nameof(ratesSourceProvider));
            this.ratesStorageService = ratesStorageService ?? throw new ArgumentNullException(nameof(ratesStorageService));
            this.invinvalidateMinutes = invalidateMinutes > 0 ? invalidateMinutes : 5;
        }

        /// <inheritdoc cref="IRatesService"/>
        public async Task<IEnumerable<ExpiredRateInfo>> GetRates(string from, string to = null)
        {
            IEnumerable<ExpiredRateInfo> expiredRates = null;

            expiredRates = to != null ? this.ratesStorageService.GetRates(from, to) : this.ratesStorageService.GetRates(from);

            if (expiredRates.Count() != 0)
            {
                return expiredRates;
            }

            var expireDate = DateTime.Now.AddMinutes(this.invinvalidateMinutes);
            expireDate = new DateTime(expireDate.Year, expireDate.Month, expireDate.Day, expireDate.Hour, expireDate.Minute, 0);
            var rates = await this.ratesSourceProvider.GetRatesAsync(from);
            expiredRates = rates
                .Select(o => new ExpiredRateInfo() { To = o.To, Rate = o.Rate, ExpireAt = expireDate })
                .ToList();
            this.ratesStorageService.SaveRates(from, expiredRates);

            return to == null ? expiredRates : expiredRates.Where(o => o.To == to);
        }
    }
}