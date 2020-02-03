namespace RatesStorageService.Entities
{
    using System;

    public class RatesHistory
    {
        public RatesHistory(string from, string to, decimal rate, DateTime invalidateDate)
        {
            this.From = from;
            this.To = to;
            this.Rate = rate;
            this.InvalidateDate = invalidateDate;
        }

        [Obsolete("ORM only", true)]
        protected RatesHistory()
        {
        }

        public virtual string From { get; protected set; }

        public virtual string To { get; protected set; }

        public virtual decimal Rate { get; protected set; }

        public virtual DateTime InvalidateDate { get; protected set; }

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

        public override int GetHashCode()
        {
            unchecked
            {
                return this.From.GetHashCode()
                       ^ this.To.GetHashCode()
                       ^ this.InvalidateDate.GetHashCode();
            }
        }

        protected bool Equals(RatesHistory obj)
        {
            return this.From == obj.From
                   && this.To == obj.To
                   && this.InvalidateDate == obj.InvalidateDate;
        }
    }
}