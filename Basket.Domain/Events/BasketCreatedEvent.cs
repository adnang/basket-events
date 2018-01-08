using System;

namespace Basket.Domain.Events
{
    public class BasketCreatedEvent : Event
    {
        public string Customer { get; }
        public string Country { get; }
        public DateTimeOffset CreatedAt { get; }

        public BasketCreatedEvent(Guid basketId, string customer, string country, DateTimeOffset createdAt)
        {
            AggregateId = basketId;
            Customer = customer;
            Country = country;
            CreatedAt = createdAt;
        }
    }
}