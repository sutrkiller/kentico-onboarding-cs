using System;
using System.Collections.Generic;
using ItemsListApp.Contracts.Models;
using NUnit.Framework;

namespace ItemsListApp.Api.UnitTests.Helpers
{
    internal class ItemTestCaseBuilder
    {
        private Item _item;
        private readonly HashSet<string> _invalidParts = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public ItemTestCaseBuilder(Item item = null)
        {
            _item = item;
        }

        public ItemTestCaseBuilder Id(Guid id)
        {
            CreateIfNotExists();
            _item.Id = id;
            return this;
        }

        public ItemTestCaseBuilder Text(string text)
        {
            CreateIfNotExists();
            _item.Text = text;
            return this;
        }

        public ItemTestCaseBuilder InvalidParts(params string[] invalidParts)
        {
            _invalidParts.UnionWith(invalidParts);
            return this;
        }

        public TestCaseData Build()
        {
         return new TestCaseData(_item, _invalidParts);
        }

        private void CreateIfNotExists()
        {
            if (_item == null)
            {
                _item = new Item();
            }
        }
    }
}
