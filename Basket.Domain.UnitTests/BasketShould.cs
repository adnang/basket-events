using System;
using System.Linq;
using Basket.Domain.Events;
using Basket.Domain.UnitTests.Framework;
using Xunit;

namespace Basket.Domain.UnitTests
{
    public class BasketShould
    {
        private const string Customer = "customer";
        private const string Country = "GB";

        [Fact]
        public void BeCreatedSuccessfully()
        {
            var basket = Basket.Initialize(Customer, Country);

            basket.ShouldHaveUncommitted<BasketCreatedEvent>()
                .WithProperty(e => e.Customer, Customer)
                .WithProperty(e => e.Country, Country)
                .WithNotNull(e => e.AggregateId)
                .AssertAll();
        }
        
        [Fact]
        public void AddProduct(){
            var basketId = Guid.NewGuid();
            var basket = new Basket().LoadFromHistory<Basket>(new []
            {
                new BasketCreatedEvent(basketId, Customer, Country, DateTimeOffset.UtcNow)
            });
            
            basket.AddVariant(new Variant
            {
                ProductId = 1234,
                VariantId = 1235,
                Description = "boots",
                GbpPrice = 12.99
            }, 1);

            basket.ShouldHaveUncommitted<NewProductAddedEvent>()
                .WithProperty(e => e.AggregateId, basketId)
                .WithProperty(e => e.ProductBasketItem.VariantId, 1235)
                .AssertAll();
        }
        
        [Fact]
        public void IncreaseQuantity_WhenAddingExistingProduct(){
            var basketId = Guid.NewGuid();
            var itemId = Guid.NewGuid();
            var basket = new Basket().LoadFromHistory<Basket>(new Event[]
            {
                new BasketCreatedEvent(basketId, Customer, Country, DateTimeOffset.UtcNow),
                new NewProductAddedEvent(basketId, new ProductBasketItem
                {
                    ProductId = 1234,
                    VariantId = 1235,
                    Description = "boots",
                    ItemId = itemId,
                    Price = new ProductPrice(12.99, "£"),
                    Quantity = 1
                }) 
            });

            basket.AddVariant(new Variant
            {
                ProductId = 1234,
                VariantId = 1235,
                Description = "boots",
                GbpPrice = 15.99
            }, 3);

            basket.ShouldHaveUncommitted<ExistingProductUpdatedEvent>()
                .WithProperty(e => e.ItemId, itemId)
                .WithProperty(e => e.Quantity, 3)
                .WithProperty(e => e.Item.VariantId, 1235)
                .WithProperty(e => e.Item.GbpPrice, 15.99)
                .AssertAll();
        }
        
        [Fact]
        public void ReturnMaxQuantityExceeded_WhenAddProductExceedsMaxQuantity(){
            var basketId = Guid.NewGuid();
            var basket = new Basket().LoadFromHistory<Basket>(new []
            {
                new BasketCreatedEvent(basketId, Customer, Country, DateTimeOffset.UtcNow)
            });

            var addProductResult = basket.AddVariant(new Variant
            {
                ProductId = 1234,
                VariantId = 1235,
                Description = "boots",
                GbpPrice = 15.99
            }, 11);

            Assert.IsAssignableFrom<AddProductResult.QuantityExceeded>(addProductResult);
            Assert.Equal(addProductResult.Messages.First(), "Maximum item quantity exceeded by 1.");
            Assert.Empty(basket.GetUncommittedChanges());
        }
    }
}