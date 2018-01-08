using System;
using System.Collections.Generic;
using System.Linq;
using Basket.Domain.Events;

namespace Basket.Domain
{
    public class Basket : AggregateRoot
    {
        private const int MaximumQuantityForItem = 10;

        public static Basket Initialize(string customer, string country)
        {
            var basket = new Basket();
            basket.ApplyChange(
                new BasketCreatedEvent(
                    Guid.NewGuid(),
                    customer,
                    country, 
                    DateTimeOffset.UtcNow));
            return basket;
        }

        public AddProductResult AddVariant(Variant item, int quantity)
        {
            if (quantity > MaximumQuantityForItem)
            {
                return new AddProductResult.QuantityExceeded(quantity - MaximumQuantityForItem);
            }

            var matchingItem = Items.Cast<ProductBasketItem>()
                .FirstOrDefault(x => x.VariantId == item.VariantId);

            if (matchingItem != null)
            {
                return TryUpdate(item, quantity, matchingItem);
            }

            RemoveMatchingExpiredItems(item);
            
            ApplyChange(new NewProductAddedEvent(Id, ProductBasketItem.Create(item, quantity)));
            return new AddProductResult.NewProductAdded();
        }

        private void RemoveMatchingExpiredItems(Variant item)
        {
            if (ExpiredItems.Any(x => x.VariantId == item.VariantId))
            {
                ApplyChange(new ExpiredItemDeletedEvent(Id, item.VariantId));
            }
        }

        private AddProductResult TryUpdate(Variant item, int quantity, ProductBasketItem matchingItem)
        {
            var newQuantity = matchingItem.Quantity + quantity;

            if (newQuantity > MaximumQuantityForItem)
            {
                ApplyChange(new ExistingProductUpdatedEvent(Id, matchingItem.ItemId, item, MaximumQuantityForItem));
                return new AddProductResult.QuantityExceeded(newQuantity - MaximumQuantityForItem);
            }

            ApplyChange(new ExistingProductUpdatedEvent(Id, matchingItem.ItemId, item, quantity));
            return new AddProductResult.ProductUpdated();
        }

        public void MarkReserved(Guid itemId, DateTimeOffset reservationExpires)
        {
            ApplyChange(new ProductBasketItemReserved(Id, itemId, reservationExpires));
        }
        
        internal void Apply(BasketCreatedEvent @event)
        {
            Id = @event.AggregateId;
            Customer = @event.Customer;
            Country = @event.Country;
            Items = new List<BasketItem>();
            ExpiredItems = new List<ExpiredProductBasketItem>();
            CreatedAt = @event.CreatedAt;
        }

        internal void Apply(NewProductAddedEvent @event)
        {
            Items.Add(@event.ProductBasketItem);
        }

        internal void Apply(ExistingProductUpdatedEvent @event)
        {
            var productBasketItem = FindProduct(@event.ItemId);
            productBasketItem?.Update(@event);
        }

        internal void Apply(ExpiredItemDeletedEvent @event)
        {
            ExpiredItems.Remove(ExpiredItems.FirstOrDefault(i => i.VariantId == @event.VariantId));
        }

        internal void Apply(ProductBasketItemReserved @event)
        {
            var productBasketItem = FindProduct(@event.ItemId);
            productBasketItem?.MarkReserved(@event);
        }

        private ProductBasketItem FindProduct(Guid itemId)
        {
            return Items.Find(i => i.ItemId == itemId) as ProductBasketItem;
        }
        
        public string Customer { get; set; }
        public string Country { get; set; }
        public List<BasketItem> Items { get; set; }
        public List<ExpiredProductBasketItem> ExpiredItems { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

    }
}