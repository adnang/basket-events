using System;

namespace Basket.Domain.Events
{
    public class ProductBasketItemReserved : Event
    {
        public Guid ItemId { get; }
        public DateTimeOffset ReservationExpires { get; }

        public ProductBasketItemReserved(Guid basketId, Guid itemId, DateTimeOffset reservationExpires)
        {
            ItemId = itemId;
            ReservationExpires = reservationExpires;
            AggregateId = basketId;
        }
    }
}