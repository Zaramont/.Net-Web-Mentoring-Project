using Catalog.BLL.Models;
using Catalog.BLL.Services;

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySiteRESTApi.Controllers
{
    [Route("api/v1/products")]
    [ApiController]
    public class ProductsApiController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsApiController(IProductService context)
        {
            _productService = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts([FromQuery] int limit)
        {
            if (limit <= 0) limit = 30;
            var Products = _productService.GetProducts(limit)
                .Select(x => new Product
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    CategoryId = x.CategoryId,
                    QuantityPerUnit = x.QuantityPerUnit,
                    ReorderLevel = x.ReorderLevel,
                    UnitPrice = x.UnitPrice,
                    UnitsInStock = x.UnitsInStock,
                    UnitsOnOrder = x.UnitsOnOrder,
                    Discontinued = x.Discontinued,
                });
            return Ok(Products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = _productService.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            var productToUpdate = _productService.GetProductById(id);

            if (productToUpdate == null)
            {
                return NotFound();
            }
            productToUpdate.ProductName = product.ProductName;
            productToUpdate.UnitsOnOrder = product.UnitsOnOrder;
            productToUpdate.Discontinued = product.Discontinued;
            productToUpdate.UnitPrice = product.UnitPrice;
            productToUpdate.UnitsInStock = product.UnitsInStock;
            productToUpdate.QuantityPerUnit = product.QuantityPerUnit;
            productToUpdate.CategoryId = product.CategoryId;

            _productService.UpdateProduct(productToUpdate);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            var createdProduct = _productService.AddProduct(
                new Catalog.DAL.Entities.Product
                {
                    ProductName = product.ProductName,
                    CategoryId = product.CategoryId,
                    Discontinued = product.Discontinued,
                    QuantityPerUnit = product.QuantityPerUnit,
                    UnitPrice = product.UnitPrice,
                    UnitsInStock = product.UnitsInStock,
                    UnitsOnOrder = product.UnitsOnOrder,
                    ReorderLevel = product.ReorderLevel,
                });

            return CreatedAtAction("PostProduct", new { id = createdProduct.ProductId }, createdProduct);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            _productService.DeleteProduct(id);
            return NoContent();
        }
    }
}
