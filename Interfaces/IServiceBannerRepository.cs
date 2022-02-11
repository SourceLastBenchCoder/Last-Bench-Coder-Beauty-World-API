using Last.Bench.Coder.Beauty.World.Entity;

namespace Last.Bench.Coder.Beauty.World.Interfaces
{
    public interface IServiceBannerRepository : IGenericRepository<ServiceBanner>
    {
        public IEnumerable<ServiceBanner> GetAllByServiceId(int ServiceId);
    }
}