using Last.Bench.Coder.Beauty.World.DataContext;
using Last.Bench.Coder.Beauty.World.Entity;
using Last.Bench.Coder.Beauty.World.Interfaces;

namespace Last.Bench.Coder.Beauty.World.Repository
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationContext context) : base(context)
        {
        }

        public IEnumerable<Category> GetAllByText(string SearchKey)
        {
            return _context.Category.Where(z => z.Title.ToLower().Contains(SearchKey.ToLower())
            || z.Description.ToLower().Contains(SearchKey.ToLower())).ToList();
        }
    }
}