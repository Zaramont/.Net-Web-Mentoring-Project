using Catalog.DAL.Entities;
using Catalog.DAL.Repositories;
using System.Collections.Generic;

namespace Catalog.DAL.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        public void Delete(int id);
    }
}
