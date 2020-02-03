namespace RatesServiceTest
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentNHibernate.Testing.Values;
    using Moq;
    using NHibernate;
    using NUnit.Framework;
    using Rates.Common;
    using RatesService;
    using RatesSources.Common;
    using RatesStorageService;

    [TestFixture]
    public class RatesServiceTest
    {
        [Test]
        public void RatesService_АргументыУказаны_ЭкземплярСоздан()
        {
            var ratesSourceProvider = Mock.Of<IRatesSourceProvider>();
            var ratesStorageService = Mock.Of<IRatesStorageService>();
            Assert.DoesNotThrow(() => new RatesService(ratesSourceProvider, ratesStorageService, 10));
        }

        [Test]
        public void RatesService_АргументыНеУказаны_ArgumentNullException()
        {
            var ratesSourceProvider = Mock.Of<IRatesSourceProvider>();
            var ratesStorageService = Mock.Of<IRatesStorageService>();
            Assert.Throws<ArgumentNullException>(() => new RatesService(null, ratesStorageService, 10));
            Assert.Throws<ArgumentNullException>(() => new RatesService(ratesSourceProvider, null, 10));
        }

        [Test]
        public void GetRates_ДанныеЕстьВБД_ВозвращаетсяРезультат()
        {
            var list = new List<ExpiredRateInfo>()
            {
                new ExpiredRateInfo()
                {
                    To = "RUB",
                    Rate = 10,
                    ExpireAt = DateTime.Today
                }
            };

            var ratesSourceProvider = Mock.Of<IRatesSourceProvider>();
            var ratesStorageService = new Mock<IRatesStorageService>();
            ratesStorageService.Setup(o => o.GetRates(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(list);
            var service = new RatesService(ratesSourceProvider, ratesStorageService.Object, 10);

            var rates = service.GetRates("from", "to").Result;
            CollectionAssert.AreEqual(list, rates);
        }

        [Test]
        public void GetRates_ДанныеБерутсяИзIRatesSourceProvider_ВозвращаетсяРезультат()
        {
            IEnumerable<RateInfo> list = new List<RateInfo>()
            {
                new RateInfo()
                {
                    To = "RUB",
                    Rate = 10,
                }
            };
            var result = new Task<IEnumerable<RateInfo>>(() => list);

            var ratesStorageService = new Mock<IRatesStorageService>();
            ratesStorageService.Setup(o => o.SaveRates(It.IsAny<string>(), It.IsAny<IEnumerable<ExpiredRateInfo>>()))
                .Verifiable();
            var ratesSourceProvider = new Mock<IRatesSourceProvider>();
            ratesSourceProvider.Setup(o => o.GetRatesAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(list));

            var service = new RatesService(ratesSourceProvider.Object, ratesStorageService.Object, 10);

            var rates = service.GetRates("USD", "RUB").Result;
            Assert.IsNotEmpty(rates);
            ratesStorageService.Verify();
        }
    }
}