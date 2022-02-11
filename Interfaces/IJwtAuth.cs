using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Last.Bench.Coder.Beauty.World.Interfaces
{
    public interface IJwtAuth
    {
        string Authentication(string username);
    }
}