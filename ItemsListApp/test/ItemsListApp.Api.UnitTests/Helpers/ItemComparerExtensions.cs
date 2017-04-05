using System;
using System.Collections.Generic;
using ItemsListApp.Contracts.Models;
using NUnit.Framework.Constraints;

namespace ItemsListApp.Api.UnitTests.Helpers
{
    internal static class ItemComparerExtensions
    {
        private static Lazy<ItemComparer> ItemComparerInstance { get; } = new Lazy<ItemComparer>();

        public static Constraint UsingItemComparer(this EqualConstraint constraint)
            => constraint.Using(ItemComparerInstance.Value);

        private sealed class ItemComparer : IEqualityComparer<Item>
        {
            public bool Equals(Item x, Item y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Id.Equals(y.Id) && string.Equals(x.Text, y.Text);
            }

            public int GetHashCode(Item obj)
            {
                unchecked
                {
                    return (obj.Id.GetHashCode() * 397) ^ (obj.Text?.GetHashCode() ?? 0);
                }
            }
        }
    }
}