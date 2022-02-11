using Last.Bench.Coder.Beauty.World.DataContext;
using Last.Bench.Coder.Beauty.World.Entity;
using Last.Bench.Coder.Beauty.World.Entity.Enums;
using Last.Bench.Coder.Beauty.World.Interfaces;

namespace Last.Bench.Coder.Beauty.World.Repository
{
    public class AdminRepository : GenericRepository<Admin>, IAdminRepository
    {
        public AdminRepository(ApplicationContext context) : base(context)
        {
        }

        public IEnumerable<Admin> GetAllByText(string SearchKey)
        {
            return _context.Admin.Where(z => z.FullName.ToLower().Contains(SearchKey.ToLower())
            || z.EmailId.ToLower().Contains(SearchKey.ToLower())
            || z.LoginId.ToLower().Contains(SearchKey.ToLower())
            || z.Role.ToLower().Contains(SearchKey.ToLower())).ToList();
        }

        public Admin Login(string LoginId, string Password)
        {
            return _context.Admin.Where(z => z.LoginId == LoginId && z.Password == Password & z.Status == eStatus.Active.ToString()).FirstOrDefault();
        }
    }
}