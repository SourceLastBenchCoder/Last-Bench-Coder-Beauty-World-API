using Last.Bench.Coder.Beauty.World.Entity;

namespace Last.Bench.Coder.Beauty.World.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        IEnumerable<Category> GetAllByText(string SearchKey);
    }
}