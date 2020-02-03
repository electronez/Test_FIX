namespace RatesStorageServiceTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Moq;
    using NHibernate;
    using NUnit.Framework;
    using Rates.Common;
    using RatesStorageService;
    using RatesStorageService.Entities;

    [TestFixture]
    public class RatesStorageServiceTest
    {
        [Test]
        public void RatesStorageService_АргументыУказаны_ЭкземплярСоздан()
        {
            var sessionFactory = Mock.Of<ISessionFactory>();
            Assert.DoesNotThrow(() => new RatesStorageService(sessionFactory));
        }

        [Test]
        public void RatesStorageService_АргументыНеУказаны_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new RatesStorageService(null));
        }

        [Test]
        public void RegisterMapping_АргументНеУказан_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => RatesStorageService.RegisterMapping(null));
        }

        [Test]
        public void GetRates_УказанаПродаваемаяВалюта_ВозвращаетСписокКурсовВалют()
        {
            var to = "to";
            var from = "from";
            decimal rateValue = 15;
            var list = new List<RatesHistory>();
            list.Add(new RatesHistory(from, to, rateValue, DateTime.Today.AddDays(1)));
            list.Add(new RatesHistory("other", "other", 10, DateTime.Today.AddDays(1)));
            
            var session = new Mock<ISession>();
            session.Setup(o => o.Query<RatesHistory>()).Returns(list.AsQueryable());

            var sessionFactory = Mock.Of<ISessionFactory>(o => o.OpenSession() == session.Object);
            var service = new RatesStorageService(sessionFactory);

            var result = service.GetRates(from);

            Assert.AreEqual(1, result.Count());
            var rate = result.First();
            Assert.AreEqual(to, rate.To);
            Assert.AreEqual(rateValue, rate.Rate);
        }

        [Test]
        public void GetRates_УказаныВалюты_ВозвращаетСписокКурсовВалют()
        {
            var to = "to";
            var from = "from";
            decimal rateValue = 15;
            var list = new List<RatesHistory>();
            list.Add(new RatesHistory(from, to, rateValue, DateTime.Today.AddDays(1)));
            list.Add(new RatesHistory(from, "other", 10, DateTime.Today.AddDays(1)));

            var session = new Mock<ISession>();
            session.Setup(o => o.Query<RatesHistory>()).Returns(list.AsQueryable());

            var sessionFactory = Mock.Of<ISessionFactory>(o => o.OpenSession() == session.Object);
            var service = new RatesStorageService(sessionFactory);

            var result = service.GetRates(from, to);

            Assert.AreEqual(1, result.Count());
            var rate = result.First();
            Assert.AreEqual(to, rate.To);
            Assert.AreEqual(rateValue, rate.Rate);
        }

        [Test]
        public void SaveRates_СохраняетЗаписьВБД()
        {
            var list = new List<RatesHistory>();

            var from = "from";
            var expiredRate = new ExpiredRateInfo()
            {
                To = "to",
                Rate = 10,
                ExpireAt = DateTime.Today
            };

            var session = new Mock<ISession>();
            session.Setup(o => o.SaveOrUpdate(It.IsAny<RatesHistory>())).Callback((object o) => list.Add((RatesHistory)o));
            session.Setup(o => o.BeginTransaction()).Returns(Mock.Of<ITransaction>());

            var sessionFactory = Mock.Of<ISessionFactory>(o => o.OpenSession() == session.Object);
            var service = new RatesStorageService(sessionFactory);

            service.SaveRates(from, new ExpiredRateInfo[] { expiredRate  });

            Assert.IsNotEmpty(list);
            var rateHistory = list.First();
            Assert.AreEqual(from, rateHistory.From);
            Assert.AreEqual(expiredRate.To, rateHistory.To);
            Assert.AreEqual(expiredRate.Rate, rateHistory.Rate);
            Assert.AreEqual(expiredRate.ExpireAt, rateHistory.InvalidateDate);
        }
    }
}