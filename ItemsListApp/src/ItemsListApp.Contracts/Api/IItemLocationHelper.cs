using System;

namespace ItemsListApp.Contracts.Api
{
    public interface IItemLocationHelper
    {
        string CreateLocation(Guid id);
    }
}
