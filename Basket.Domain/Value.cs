namespace Basket.Domain
{
    public abstract class Value<T>
    {
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ProductPrice) obj);
        }

        public override int GetHashCode()
        {
            return HashCode();
        }

        protected abstract bool Equals(T other);

        protected abstract int HashCode();
    }
}