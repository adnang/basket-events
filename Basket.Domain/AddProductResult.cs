using System.Collections.Generic;

namespace Basket.Domain
{
    public class AddProductResult
    {
        public List<string> Messages { get; } = new List<string>();
        
        public class QuantityExceeded : AddProductResult
        {
            public QuantityExceeded(int exceededBy)
            {
                Messages.Add($"Maximum item quantity exceeded by {exceededBy}.");
            }
        }

        public class ProductUpdated : AddProductResult
        {
        }

        public class NewProductAdded : AddProductResult
        {
        }
    }
}