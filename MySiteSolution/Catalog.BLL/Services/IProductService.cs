using Catalog.DAL.Entities;
using System.Collections.Generic;

namespace Catalog.BLL.Services
{
    public interface IProductService
    {
        IList<Product> GetProducts(int limit);

        Product GetProductById(int id);
        void AddProduct(Product entity);
        void DeleteProduct(int id);
        void UpdateProduct(Product entity);
    }
}
