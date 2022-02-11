using Last.Bench.Coder.Beauty.World.DataContext;
using Last.Bench.Coder.Beauty.World.Entity;
using Last.Bench.Coder.Beauty.World.Interfaces;

namespace Last.Bench.Coder.Beauty.World.Repository
{
    public class ServiceRepository : GenericRepository<Service>, IServiceRepository
    {
        public ServiceRepository(ApplicationContext context) : base(context)
        {
        }

        public IEnumerable<Service> GetAllByText(string SearchKey)
        {
            return _context.Service.Where(z => z.Title.ToLower().Contains(SearchKey.ToLower())
            || z.Description.ToLower().Contains(SearchKey.ToLower())).ToList();
        }

        public int GetMaxServiceId()
        {
            int maxServiceId = 0;

            maxServiceId = _context.Service
           .Select(t => t.ServiceId)
           .DefaultIfEmpty().ToList().Max();
           
            maxServiceId = (maxServiceId == 0) ? 1 : maxServiceId + 1;

            return maxServiceId;
        }
    }
}