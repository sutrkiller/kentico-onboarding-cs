using System;
using System.Threading.Tasks;
using ItemsListApp.Contracts.Services;

namespace ItemsListApp.Services.StaticWrappers
{
    internal class IdentifierService : IIdentifierService
    {
        public Task<Guid> GenerateIdAsync() 
            => Task.FromResult(Guid.NewGuid());
    }
}
