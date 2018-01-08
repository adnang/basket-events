using System;
using Basket.Domain.Events;

namespace Basket.Domain
{
    public class ProductBasketItem : BasketItem
    {
        public int VariantId { get; set; }
        public int ProductId { get; set; }
        public string Description { get; set; }
        public ProductPrice Price { get; set; }
        public int Quantity { get; set; }
        public DateTimeOffset? ReservationExpires { get; set; }
        public ReservationStatus ReservationStatus { get; set; }

        public static ProductBasketItem Create(Variant variant, int quantity)
        {
            return new ProductBasketItem
            {
                ItemId = Guid.NewGuid(),
                VariantId = variant.VariantId,
                ProductId = variant.ProductId,
                Description = variant.Description,
                Price = new ProductPrice(variant.GbpPrice, Currency.Gbp),
                Quantity = quantity,
                ReservationStatus = ReservationStatus.Pending
            };
        }

        public void Update(ExistingProductUpdatedEvent @event)
        {
            var item = @event.Item;
            Description = item.Description;
            Price = new ProductPrice(item.GbpPrice, Currency.Gbp);
            Quantity = @event.Quantity;
        }

        public void MarkReserved(ProductBasketItemReserved @event)
        {
            ReservationExpires = @event.ReservationExpires;
            ReservationStatus = ReservationStatus.Reserved;
        }
    }
}