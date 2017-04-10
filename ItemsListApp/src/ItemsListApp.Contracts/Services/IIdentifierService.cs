using System;
using System.Threading.Tasks;

namespace ItemsListApp.Contracts.Services
{
    public interface IIdentifierService
    {
        Task<Guid> GenerateIdAsync();
    }
}