using System;

namespace Basket.Domain.Events
{
    public class ExistingProductUpdatedEvent : Event
    {
        public ExistingProductUpdatedEvent(Guid basketId, Guid itemId, Variant item, int quantity)
        {
            AggregateId = basketId;
            ItemId = itemId;
            Item = item;
            Quantity = quantity;
        }

        public Guid ItemId { get; }
        public Variant Item { get; }
        public int Quantity { get; }
    }
}