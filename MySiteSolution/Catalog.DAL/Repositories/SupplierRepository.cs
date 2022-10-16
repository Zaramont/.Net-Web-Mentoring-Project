using Catalog.DAL.DataContext;
using Catalog.DAL.Entities;

namespace Catalog.DAL.Repositories
{
    public class SupplierRepository : GenericRepository<Supplier>, ISupplierRepository
    {
        private CatalogDataContext catalogDBContext;
        public SupplierRepository(CatalogDataContext context)
            : base(context)
        {
            catalogDBContext = context;
        }
    }
}
