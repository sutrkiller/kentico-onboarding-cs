using System;
using System.Threading.Tasks;

namespace ItemsListApp.Contracts.Services
{
    public interface IDateTimeService
    {
        Task<DateTime> GetCurrentDateAsync();
    }
}
