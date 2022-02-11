using Last.Bench.Coder.Beauty.World.DataContext;
using Last.Bench.Coder.Beauty.World.Entity;
using Last.Bench.Coder.Beauty.World.Interfaces;

namespace Last.Bench.Coder.Beauty.World.Repository
{
    public class ServiceBannerRepository : GenericRepository<ServiceBanner>, IServiceBannerRepository
    {
        public ServiceBannerRepository(ApplicationContext context) : base(context)
        {
        }

        public IEnumerable<ServiceBanner> GetAllByServiceId(int ServiceId)
        {
            return _context.ServiceBanner.Where(b => b.ServiceId == ServiceId);
        }
    }
}