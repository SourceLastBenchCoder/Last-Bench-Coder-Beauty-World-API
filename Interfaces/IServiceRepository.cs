using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Last.Bench.Coder.Beauty.World.Entity;

namespace Last.Bench.Coder.Beauty.World.Interfaces
{
    public interface IServiceRepository : IGenericRepository<Service>
    {
        IEnumerable<Service> GetAllByText(string SearchKey);
        int GetMaxServiceId();
    }
}