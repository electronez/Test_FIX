namespace RatesWebService.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using RatesService;
    using RatesWebService.Models;

    /// <summary>
    /// Контроллер для получения курсов валют
    /// </summary>
    public class RateController : ControllerBase
    {
        private readonly IRatesService ratesService;
        public RateController(IRatesService ratesService)
        {
            this.ratesService = ratesService ?? throw new ArgumentNullException(nameof(ratesService));
        }

        /// <summary>
        /// Получение курсов валют
        /// </summary>
        /// <param name="request">Информация о запросе</param>
        [Route("rates/{from}/{to?}")]
        [HttpGet]
        public async Task<IActionResult> GetRates([FromRoute] RatesRequest request)
        {
            var rates = await ratesService.GetRates(request.From, request.To);
            return this.Ok(new RatesResponse()
            {
                From = request.From,
                Rates = rates.ToArray()
            });
        }
    }
}
