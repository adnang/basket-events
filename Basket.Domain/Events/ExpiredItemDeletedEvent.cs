using System;

namespace Basket.Domain.Events
{
    public class ExpiredItemDeletedEvent : Event
    {
        public ExpiredItemDeletedEvent(Guid basketId, int variantId)
        {
            AggregateId = basketId;
            VariantId = variantId;
        }

        public int VariantId { get; }
    }
}