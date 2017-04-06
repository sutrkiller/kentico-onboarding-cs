using System;
using ItemsListApp.Contracts.Models;

namespace ItemsListApp.Api.UnitTests.Helpers
{
    internal class ItemPutTestCaseBuilder : ItemTestCaseBuilder
    {
        protected override Item CreateValidItem()
            => new Item
            {
                Id = new Guid("479867AA-1761-43D6-A2DD-0DCF07142D89"),
                Text = "Something realllly creative",
            };
    }
}