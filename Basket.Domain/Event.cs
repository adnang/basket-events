using System;

namespace Basket.Domain
{
    public abstract class Event
    {
        public Guid AggregateId { get; set; }
    }
}