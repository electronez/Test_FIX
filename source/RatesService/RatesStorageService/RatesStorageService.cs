namespace RatesStorageService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentNHibernate.Cfg;
    using global::RatesStorageService.Entities;
    using global::RatesStorageService.Mappings;
    using NHibernate;
    using NHibernate.Transaction;
    using Rates.Common;

    public class RatesStorageService : IRatesStorageService
    {
        private readonly ISessionFactory sessionFactory;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="RatesStorageService"/>
        /// </summary>
        /// <param name="sessionFactory">Фабрика сессий</param>
        public RatesStorageService(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory ?? throw new ArgumentNullException(nameof(sessionFactory));
        }

        /// <summary>
        /// Регистрирует маппинги сервиса
        /// </summary>
        /// <param name="mappingConfiguration">Конфигурация маппинга</param>
        public static void RegisterMapping(MappingConfiguration mappingConfiguration)
        {
            if (mappingConfiguration == null)
            {
                throw new ArgumentNullException(nameof(mappingConfiguration));
            }

            mappingConfiguration.FluentMappings.AddFromAssemblyOf<RatesHistoryMap>();
        }

        /// <summary>
        /// Получение курса валют из БД
        /// </summary>
        /// <param name="from">Продаваемая валюта</param>
        /// <param name="to">Покупаемая валюта</param>
        /// <returns>Коллекция курса валют</returns>
        public IEnumerable<ExpiredRateInfo> GetRates(string from, string to)
        {
            return this.GetRates(from).Where(o => o.To == to);
        }

        /// <summary>
        /// Получение курса валют из БД
        /// </summary>
        /// <param name="from">Продаваемая валюта</param>
        /// <returns>Коллекция курса валют</returns>
        public IEnumerable<ExpiredRateInfo> GetRates(string from)
        {
            using (var session = this.sessionFactory.OpenSession())
            {
                return session.Query<RatesHistory>()
                    .Where(o => o.From == from && o.InvalidateDate > DateTime.Now)
                    .Select(o => new ExpiredRateInfo() { To = o.To, Rate = o.Rate, ExpireAt = o.InvalidateDate })
                    .ToList();
            }
        }

        /// <summary>
        /// Сохранение курса валют в БД
        /// </summary>
        /// <param name="from">Продаваемая валюта</param>
        /// <param name="rates">Курс валют</param>
        public void SaveRates(string from, IEnumerable<ExpiredRateInfo> rates)
        {
            using (var session = this.sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    foreach (var rateInfo in rates)
                    {
                        session.SaveOrUpdate(new RatesHistory(from, rateInfo.To, rateInfo.Rate, rateInfo.ExpireAt));
                    }

                    transaction.Commit();
                }
            }
        }
    }
}