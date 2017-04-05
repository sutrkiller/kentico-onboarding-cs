using System;
using System.Threading.Tasks;
using ItemsListApp.Contracts.Services;

namespace ItemsListApp.Services.Identity
{
    internal class IdGeneratorService : IIdGeneratorService
    {
        public Task<Guid> GenerateIdAsync() 
            => Task.FromResult(Guid.NewGuid());
    }
}
