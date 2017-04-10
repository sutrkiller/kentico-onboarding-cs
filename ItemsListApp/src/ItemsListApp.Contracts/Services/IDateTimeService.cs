using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemsListApp.Contracts.Services
{
    public interface IDateTimeService
    {
        Task<DateTime> GetCurrentDateAsync();
    }
}
