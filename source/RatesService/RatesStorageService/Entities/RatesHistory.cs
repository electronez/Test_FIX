namespace RatesStorageService.Entities
{
    using System;

    /// <summary>
    /// Исторические замеры
    /// </summary>
    public class RatesHistory
    {
        /// <summary>
        /// Инициализирует экземпляр класса <see cref="RatesHistory"/>
        /// </summary>
        /// <param name="from">Продаваемая валюта</param>
        /// <param name="to">Покупаемая валюта</param>
        /// <param name="rate">Цена</param>
        /// <param name="invalidateDate">Дата и время истечения актуальности</param>
        public RatesHistory(string from, string to, decimal rate, DateTime invalidateDate)
        {
            this.From = from;
            this.To = to;
            this.Rate = rate;
            this.InvalidateDate = invalidateDate;
        }

        /// <summary>
        /// Инициализирует экземпляр класса <see cref="RatesHistory"/>
        /// </summary>
        [Obsolete("ORM only", true)]
        protected RatesHistory()
        {
        }

        /// <summary>
        /// Продаваемая валюта
        /// </summary>
        public virtual string From { get; protected set; }

        /// <summary>
        /// Покупаемая валюта
        /// </summary>
        public virtual string To { get; protected set; }

        /// <summary>
        /// Цена
        /// </summary>
        public virtual decimal Rate { get; protected set; }

        /// <summary>
        /// Дата и время истечения актуальности
        /// </summary>
        public virtual DateTime InvalidateDate { get; protected set; }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((RatesHistory)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return this.From.GetHashCode()
                       ^ this.To.GetHashCode()
                       ^ this.InvalidateDate.GetHashCode();
            }
        }

        /// <summary>
        /// Сравнивает два объекта типа <see cref="RatesHistory"/>
        /// </summary>
        /// <param name="obj">Объект типа <see cref="RatesHistory"/></param>
        /// <returns><see langword="true"/>-если объекты одинаковы, иначе<see langword="false"/></returns>
        protected bool Equals(RatesHistory obj)
        {
            return this.From == obj.From
                   && this.To == obj.To
                   && this.InvalidateDate == obj.InvalidateDate;
        }
    }
}