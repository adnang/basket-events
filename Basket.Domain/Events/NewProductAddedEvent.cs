using System;

namespace Basket.Domain.Events
{
    public class NewProductAddedEvent : Event
    {
        public ProductBasketItem ProductBasketItem { get; }

        public NewProductAddedEvent(Guid basketId, ProductBasketItem productBasketItem)
        {
            AggregateId = basketId;
            ProductBasketItem = productBasketItem;
        }
    }
}