using Last.Bench.Coder.Beauty.World.DataContext;
using Last.Bench.Coder.Beauty.World.Interfaces;
using Last.Bench.Coder.Beauty.World.Repository;

namespace Last.Bench.Coder.Beauty.World.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _context;
        public UnitOfWork(ApplicationContext context)
        {
            _context = context;
            Store = new StoreRepository(_context);
            Admin = new AdminRepository(_context);
            Category = new CategoryRepository(_context);
            Service = new ServiceRepository(_context);
            ServiceBanner = new ServiceBannerRepository(_context);
        }
        public IStoreRepository Store { get; private set; }
        public IAdminRepository Admin { get; private set; }
        public ICategoryRepository Category { get; private set; }
        public IServiceRepository Service { get; private set; }
        public IServiceBannerRepository ServiceBanner { get; private set; }
        public int Complete()
        {
            return _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}