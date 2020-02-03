namespace RatesStorageService.Mappings
{
    using FluentNHibernate.Mapping;
    using global::RatesStorageService.Entities;

    /// <summary>
    /// Описывает отображение класса <see cref="RatesHistory"/> в БД
    /// </summary>
    internal class RatesHistoryMap : ClassMap<RatesHistory>
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="RatesHistoryMap"/>.
        /// </summary>
        public RatesHistoryMap()
        {
            this.Schema("public");
            this.Table("rate_histories");
            this.CompositeId()
                .KeyProperty(o => o.From, "from_currency")
                .KeyProperty(o => o.To, "to_currency")
                .KeyProperty(o => o.InvalidateDate, "invalidate_date");

            this.Map(o => o.Rate, "rate");
        }
    }
}