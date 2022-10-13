using Catalog.DAL.Entities;
using Catalog.DAL.DataContext;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Catalog.DAL.Repositories
{
    public class CategoriesRepository : GenericRepository<Category>, ICategoryRepository
    {
        private CatalogDataContext catalogDBContext;
        public CategoriesRepository(CatalogDataContext context)
            : base(context)
        {
            catalogDBContext = context;
        }

        public override Category GetById(int id)
        {
            var result = catalogDBContext.Categories.AsNoTracking().Include(x => x.Products).FirstOrDefault(x => x.CategoryId == id);
            return result;
        }

        public override IList<Category> GetAll()
        {
            var result = catalogDBContext.Categories.AsNoTracking().Include(x => x.Products).ToList();
            return result;
        }

        public void Delete(int id)
        {
            var category = new Category() { CategoryId = id };
            var products = catalogDBContext.Products.Where(_=>_.CategoryId == id);
            foreach (var product in products)
            {
                catalogDBContext.Products.Attach(product);
                catalogDBContext.Products.Remove(product);
            }
            catalogDBContext.Categories.Attach(category);
            catalogDBContext.Categories.Remove(category);
            catalogDBContext.SaveChanges();
        }
    }
}
