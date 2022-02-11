using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Last.Bench.Coder.Beauty.World.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IStoreRepository Store { get; }
        IAdminRepository Admin { get; }
        ICategoryRepository Category { get; }
        IServiceRepository Service { get; }
         IServiceBannerRepository ServiceBanner { get; }
        int Complete();
    }
}