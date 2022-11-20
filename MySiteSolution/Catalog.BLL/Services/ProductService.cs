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

        public Product AddProduct(Product entity)
        {
            productRepository.Create(entity);
            return GetProductById(entity.ProductId);
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

        public IList<Product> GetProducts(int pageSize)
        {
            return GetProducts(0, pageSize);
        }

        public IList<Product> GetProducts(int pageIndex, int pageSize)
        {
            var products = new List<Product>();

            if (pageSize == 0)
            {
                products = productRepository.GetAll().ToList();
            }
            else if (pageSize > 0)
            {
                products = productRepository.GetAll().Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }

            return products;
        }

        public void UpdateProduct(Product entity)
        {
            if (entity != null) productRepository.Update(entity);
        }
    }
}
