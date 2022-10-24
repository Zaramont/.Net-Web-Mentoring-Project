using Catalog.DAL.Entities;

namespace Catalog.DAL.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        public void Delete(int id);
    }
}
