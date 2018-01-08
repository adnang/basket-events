namespace Basket.Domain
{
    public class Variant : Value<Variant>
    {
        public int VariantId { get; set; }
        public int ProductId { get; set; }
        public string Description { get; set; }
        public double GbpPrice { get; set; }

        protected override bool Equals(Variant other)
        {
            return VariantId == other.VariantId 
                   && ProductId == other.ProductId 
                   && string.Equals(Description, other.Description) 
                   && GbpPrice.Equals(other.GbpPrice);
        }

        protected override int HashCode()
        {
            unchecked
            {
                var hashCode = 17;
                hashCode = (hashCode * 397) ^ VariantId;
                hashCode = (hashCode * 397) ^ ProductId;
                hashCode = (hashCode * 397) ^ (Description?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ GbpPrice.GetHashCode();
                return hashCode;
            }
        }
    }
}