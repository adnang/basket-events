using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Basket.Domain.UnitTests.Framework
{
    public static class AggregateAssertionExtensions
    {
        public static AggregateAssertion<T> ShouldHaveUncommitted<T>(this AggregateRoot aggregateroot)
            where T : Event
        {
            return new AggregateAssertion<T>(aggregateroot);
        }
    }

    public class AggregateAssertion<T> where T : Event
    {
        private AggregateRoot _aggregateroot;
        private readonly List<Predicate<T>> _assertions;


        public AggregateAssertion(AggregateRoot aggregateroot)
        {
            _aggregateroot = aggregateroot;
            _assertions = new List<Predicate<T>>();
        }

        public AggregateAssertion<T> WithProperty(Func<T, object> accessor, object expectedValue)
        {
            _assertions.Add(e => accessor(e).Equals(expectedValue));
            return this;
        }

        public AggregateAssertion<T> WithNotNull(Func<T, object> accessor)
        {
            _assertions.Add(e => accessor(e) != null);
            return this;
        }

        public void AssertAll()
        {
            var any = 0;
            foreach (var @event in _aggregateroot.GetUncommittedChanges())
            {
                if (!(@event is T tevent)) continue;
                if (_assertions.Select(p => p(tevent)).All(p => p))
                {
                    any++;
                }
            }
            Assert.True(any > 0);
            _assertions.Clear();
            _aggregateroot = null;
        }
    }
}