using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemsListApp.Contracts.Api
{
    public interface IItemLocationHelper
    {
        string CreateLocation(Guid id);
    }
}
