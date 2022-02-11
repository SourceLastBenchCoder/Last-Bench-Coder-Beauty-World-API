using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Last.Bench.Coder.Beauty.World.Entity;

namespace Last.Bench.Coder.Beauty.World.Interfaces
{
    public interface IAdminRepository : IGenericRepository<Admin>
    {
        Admin Login(string LoginId, string Password);
        IEnumerable<Admin> GetAllByText(string SearchKey);
    }
}