namespace OpenExchangeRatesSourceTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Moq;
    using Moq.Protected;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using OpenExchangeRatesSource;

    [TestFixture]
    public class OpenExchangeSourseProviderTest
    {
        [Test]
        public void OpenExchangeSourseProvider_АргументыУказаны_ЭкземплярСоздан()
        {
            var httpClientFactory = Mock.Of<IHttpClientFactory>();
            var appId = Guid.NewGuid().ToString();
            Assert.DoesNotThrow(() => new OpenExchangeSourseProvider(httpClientFactory, appId));
        }

        [Test]
        public void RatesStorageService_АргументыНеУказаны_ArgumentNullException()
        {
            var httpClientFactory = Mock.Of<IHttpClientFactory>();
            var appId = Guid.NewGuid().ToString();

            Assert.Throws<ArgumentNullException>(() => new OpenExchangeSourseProvider(httpClientFactory, null));
            Assert.Throws<ArgumentNullException>(() => new OpenExchangeSourseProvider(null, appId));
        }

        [Test]
        public void GetRatesAsync_УказанаПродаваемаяВалюта_ВозвращаетСписокКурсовВалют()
        {
            var from = "USD";
            var to = "RUB";
            var ratesDictionary = new Dictionary<string, decimal>() {{to, 10}};

            var response = JsonConvert.SerializeObject(new OpenExchangeRateEntity()
            {
                Base = from,
                Rates = ratesDictionary
            });

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(response),
                })
                .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = OpenExchangeSourseProvider.BaseUrl,
            };

            var mockFactory = new Mock<IHttpClientFactory>();
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);
            var appId = Guid.NewGuid().ToString();
            var provider = new OpenExchangeSourseProvider(mockFactory.Object, appId);
            
            var rates = provider.GetRatesAsync(from).GetAwaiter().GetResult();
            Assert.IsNotEmpty(rates);
            var rate = rates.First();
            Assert.AreEqual(to, rate.To);
            Assert.AreEqual(ratesDictionary[to], rate.Rate);
        }
    }
}
