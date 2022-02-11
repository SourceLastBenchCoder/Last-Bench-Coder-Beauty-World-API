using Last.Bench.Coder.Beauty.World.DataContext;
using Last.Bench.Coder.Beauty.World.Entity;
using Last.Bench.Coder.Beauty.World.Interfaces;

namespace Last.Bench.Coder.Beauty.World.Repository
{
    public class StoreRepository : GenericRepository<Store>, IStoreRepository
    {
        public StoreRepository(ApplicationContext context) : base(context)
        {
        }

        public IEnumerable<Store> GetAllByText(string SearchKey)
        {
            return _context.Store.Where(z => z.StoreName.ToLower().Contains(SearchKey.ToLower())
            || z.Description.ToLower().Contains(SearchKey.ToLower())
            || z.Address.ToLower().Contains(SearchKey.ToLower())
            || z.ContactDetail.ToLower().Contains(SearchKey.ToLower())).ToList();
        }

        public IEnumerable<Store> GetAllByZipCode(string ZipCode)
        {
            return _context.Store.Where(z => z.ZipCode == ZipCode).ToList();
        }
    }
}