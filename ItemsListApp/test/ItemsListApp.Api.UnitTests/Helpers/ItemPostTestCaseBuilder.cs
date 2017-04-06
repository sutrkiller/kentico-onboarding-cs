using ItemsListApp.Contracts.Models;

namespace ItemsListApp.Api.UnitTests.Helpers
{
    internal class ItemPostTestCaseBuilder : ItemTestCaseBuilder
    {
        protected override Item CreateValidItem()
            => new Item
            {
                Text = "Something realllly creative"
            };
    }
}