using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Last.Bench.Coder.Beauty.World.Entity;

namespace Last.Bench.Coder.Beauty.World.Interfaces
{
    public interface IStoreRepository : IGenericRepository<Store>
    {
        IEnumerable<Store> GetAllByZipCode(string ZipCode);
        IEnumerable<Store> GetAllByText(string SearchKey);
    }
}