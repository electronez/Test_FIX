namespace RatesStorageService.Mappings
{
    using FluentNHibernate.Mapping;
    using global::RatesStorageService.Entities;

    internal class RatesHistoryMap : ClassMap<RatesHistory>
    {
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