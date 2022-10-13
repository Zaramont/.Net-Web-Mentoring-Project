using Catalog.DAL.DataContext;
using Catalog.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Catalog.DAL.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private CatalogDataContext catalogDBContext;
        public ProductRepository(CatalogDataContext context)
            : base(context)
        {
            catalogDBContext = context;
        }

        public override IList<Product> GetAll()
        {
            var result = catalogDBContext.Products
                .AsNoTracking()
                .Include(x => x.Category)
                .Include(x => x.Supplier)
                .ToList();
            return result;
        }

        public void Delete(int id)
        {
            var product = new Product() { ProductId = id };
            catalogDBContext.Products.Attach(product);
            catalogDBContext.Products.Remove(product);
            catalogDBContext.SaveChanges();
        }

    }
}
