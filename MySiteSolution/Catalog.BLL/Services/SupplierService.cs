using Catalog.DAL.Entities;
using Catalog.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.BLL.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository supplierRepository;

        public SupplierService(ISupplierRepository supplierRepository)
        {
            this.supplierRepository = supplierRepository ?? throw new ArgumentNullException(nameof(supplierRepository));
        }
        public IList<Supplier> GetSuppliers()
        {
            return supplierRepository.GetAll();
        }
    }
}
