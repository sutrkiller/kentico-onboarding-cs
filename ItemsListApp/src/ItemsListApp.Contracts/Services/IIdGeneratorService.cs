using System;
using System.Threading.Tasks;

namespace ItemsListApp.Contracts.Services
{
    public interface IIdGeneratorService
    {
        Task<Guid> GenerateIdAsync();
    }
}