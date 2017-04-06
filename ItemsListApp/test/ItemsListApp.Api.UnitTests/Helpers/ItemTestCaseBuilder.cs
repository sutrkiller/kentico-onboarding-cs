using System;
using System.Collections.Generic;
using ItemsListApp.Contracts.Models;
using NUnit.Framework;

namespace ItemsListApp.Api.UnitTests.Helpers
{
    internal class ItemTestCaseBuilder
    {
        private readonly Lazy<Item> _item = new Lazy<Item>(CreateNewItem);

        private static Item CreateNewItem()
            => new Item
            {
                Text = "Something realllly creative"
            };

        private readonly HashSet<string> _invalidParts = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        private Item Item => _item.Value;

        public ItemTestCaseBuilder InvalidateId(Guid id)
        {
            Item.Id = id;
            _invalidParts.Add(nameof(Item.Id));
            return this;
        }

        public ItemTestCaseBuilder InvalidateText(string text)
        {
            Item.Text = text;
            _invalidParts.Add(nameof(Item.Text));
            return this;
        }

        public TestCaseData Build()
        {
         return new TestCaseData(Item, _invalidParts);
        }

        public TestCaseData InvalidItem()
        {
            _invalidParts.Add(nameof(Item));
            return new TestCaseData(null, _invalidParts);
        }
    }
}
