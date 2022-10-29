using Catalog.BLL.Services;
using Catalog.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MyCatalogSite.Controllers;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MySiteTestProject.UnitTests.Controllers
{
    public class ProductsControllerTests
    {
        [Fact]
        public void Index_ReturnsAViewResult_WithAListOfProducts()
        {
            // Arrange
            Mock<ICategoryService> mockCategoryService = new Mock<ICategoryService>();
            Mock<IProductService> mockProductService = new Mock<IProductService>();
            Mock<ISupplierService> mockSupplierService = new Mock<ISupplierService>();
            mockProductService.Setup(repo => repo.GetProducts(It.IsAny<int>()))
                    .Returns(GetTestProducts());
            var controller = new ProductsController(mockCategoryService.Object, mockProductService.Object, mockSupplierService.Object);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Product>>(
                viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public void CreateProductGet_ReturnsCreateView()
        {
            // Arrange
            Mock<ICategoryService> mockCategoryService = new Mock<ICategoryService>();
            Mock<IProductService> mockProductService = new Mock<IProductService>();
            Mock<ISupplierService> mockSupplierService = new Mock<ISupplierService>();

            mockCategoryService.Setup(repo => repo.GetCategories()).Returns(() => {
                var list = new List<Category>();
                list.Add(new Mock<Category>().Object);

                return list;
            });
            mockSupplierService.Setup(repo => repo.GetSuppliers()).Returns(() => {
                var list = new List<Supplier>();
                list.Add(new Mock<Supplier>().Object);

                return list;
            });

            var controller = new ProductsController(mockCategoryService.Object, mockProductService.Object, mockSupplierService.Object);
            
            // Act
            var result = controller.CreateProduct();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
            Assert.True(string.IsNullOrEmpty(viewResult.ViewName) || "Index".Equals(viewResult.ViewName));
        }

        [Fact]
        public void CreateProductPost_ReturnsARedirectAndAddsProduct_WhenModelStateIsValid()
        {
            // Arrange
            Mock<ICategoryService> mockCategoryService = new Mock<ICategoryService>();
            Mock<IProductService> mockProductService = new Mock<IProductService>();
            Mock<ISupplierService> mockSupplierService = new Mock<ISupplierService>();

            mockProductService.Setup(repo => repo.AddProduct(It.IsAny<Product>()))
                .Verifiable();
            var controller = new ProductsController(mockCategoryService.Object, mockProductService.Object, mockSupplierService.Object);
            var newProduct = new Product
            {
                ProductId = 11,
                ProductName = "Test product 11",
                ReorderLevel = 111,
                UnitPrice = 111M,
                UnitsInStock = 1111,
                CategoryId = 11,
                SupplierId = 11,
            };

            // Act
            var result = controller.CreateProduct(newProduct);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            mockProductService.Verify();
        }

        [Fact]
        public void EditProductGet_ReturnsARedirect_WhenProductWithGivenIdIsNull()
        {
            // Arrange
            Mock<ICategoryService> mockCategoryService = new Mock<ICategoryService>();
            Mock<IProductService> mockProductService = new Mock<IProductService>();
            Mock<ISupplierService> mockSupplierService = new Mock<ISupplierService>();

            mockProductService.Setup(repo => repo.GetProductById(It.IsAny<int>()))
                .Verifiable();
            var controller = new ProductsController(mockCategoryService.Object, mockProductService.Object, mockSupplierService.Object);

            // Act
            var result = controller.EditProduct(It.IsAny<int>());

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            mockProductService.Verify();
        }

        [Fact]
        public void EditProductPost_ReturnsARedirect_WhenProductWithGivenIdExistsAndModelIsValid()
        {
            // Arrange
            Mock<ICategoryService> mockCategoryService = new Mock<ICategoryService>();
            Mock<IProductService> mockProductService = new Mock<IProductService>();
            Mock<ISupplierService> mockSupplierService = new Mock<ISupplierService>();

            var controller = new ProductsController(mockCategoryService.Object, mockProductService.Object, mockSupplierService.Object);
            var product = new Product
            {
                ProductId = 11,
                ProductName = "Test product 11",
                ReorderLevel = 111,
                UnitPrice = 111M,
                UnitsInStock = 1111,
                CategoryId = 11,
                SupplierId = 11,
            };

            mockProductService.Setup(repo => repo.GetProductById(It.IsAny<int>())).Returns(product)
                .Verifiable();

            // Act
            product.ProductName = "Test product zero";
            var result = controller.EditProduct(It.IsAny<int>(), product);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            mockProductService.Verify();
        }

        private IList<Product> GetTestProducts()
        {
            var products = new List<Product>();
            products.Add(new Product()
            {
                ProductId = 1,
                ProductName = "Test product 1",
                ReorderLevel = 10,
                UnitPrice = 10.40M,
                UnitsInStock = 1000,
                CategoryId = 1,
                SupplierId = 1,
            });
            products.Add(new Product()
            {
                ProductId = 2,
                ProductName = "Test product 2",
                ReorderLevel = 20,
                UnitPrice = 22,
                UnitsInStock = 1500,
                CategoryId = 2,
                SupplierId = 2,
            });
            return products;
        }
    }
}
