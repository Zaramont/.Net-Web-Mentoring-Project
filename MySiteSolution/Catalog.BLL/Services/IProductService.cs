using Catalog.DAL.Entities;
using System.Collections.Generic;

namespace Catalog.BLL.Services
{
    public interface IProductService
    {
        IList<Product> GetProducts(int limit);
        IList<Product> GetProducts(int pageIndex, int pageSize);
        Product GetProductById(int id);
        Product AddProduct(Product entity);
        void DeleteProduct(int id);
        void UpdateProduct(Product entity);
    }
}
