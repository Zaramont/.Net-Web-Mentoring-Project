using Catalog.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.BLL.Services
{
    public interface ISupplierService
    {
        IList<Supplier> GetSuppliers();
    }
}
