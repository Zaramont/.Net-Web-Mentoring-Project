using Catalog.DAL.Entities;
using Catalog.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Catalog.BLL.Services
{
    public class ProductService : IProductService
    {
        private IProductRepository productRepository;

        public ProductService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public void AddProduct(Product entity)
        {
            productRepository.Create(entity);
        }

        public void DeleteProduct(int id)
        {
            productRepository.Delete(id);
        }

        public Product GetProductById(int id)
        {
            var product = productRepository.GetById(id);
            return product;
        }

        public IList<Product> GetProducts(int limit)
        {
            var products = new List<Product>();

            if (limit == 0)
            {
                products = productRepository.GetAll().ToList();
            }
            else if (limit > 0)
            {
                products = productRepository.GetAll().Take(limit).ToList();
            }

            return products;
        }

        public void UpdateProduct(Product entity)
        {
            if (entity != null) productRepository.Update(entity);
        }
    }
}
