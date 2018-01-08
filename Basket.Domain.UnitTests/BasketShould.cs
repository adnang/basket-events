using System.Linq;
using Basket.Domain.Events;
using Xunit;

namespace Basket.Domain.UnitTests
{
    public class BasketShould
    {
        [Fact]
        public void BeCreatedSuccessfully()
        {
            var customer = "customer";
            var country = "GB";
            var basket = Basket.Initialize(customer, country);
            var uncommittedChanges = basket.GetUncommittedChanges().ToList();
            
            Assert.Contains(
                uncommittedChanges,
                e =>
                    e is BasketCreatedEvent bce 
                    && bce.Customer.Equals(customer) 
                    && bce.Country.Equals(country));
            
            Assert.Single(uncommittedChanges);
        }
    }
}