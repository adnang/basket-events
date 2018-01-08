namespace Basket.Domain
{
    public class ProductPrice : Value<ProductPrice>
    {
        public double Value { get; }
        public string FormattedText { get; }

        public ProductPrice(double value, string currencySymbol)
        {
            Value = value;
            FormattedText = $"{currencySymbol}{value:f2}";
        }

        protected override bool Equals(ProductPrice other)
        {
            return Value.Equals(other.Value) && string.Equals(FormattedText, other.FormattedText);
        }

        protected override int HashCode()
        {
            unchecked
            {
                return (Value.GetHashCode() * 397) ^ (FormattedText != null ? FormattedText.GetHashCode() : 0);
            }
        }
    }
}