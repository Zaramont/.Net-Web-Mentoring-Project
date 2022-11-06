using Catalog.BLL.Services;
using Catalog.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MyCatalogSite.Controllers;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MySiteTestProject.UnitTests.Controllers
{
    public class CategoriesControllerTests
    {
        [Fact]
        public void Index_ReturnsAViewResult_WithAListOfCategories()
        {
            // Arrange
            var logger = new Mock<ILogger<CategoriesController>>();
            var mockCategoryService = new Mock<ICategoryService>();
            mockCategoryService.Setup(repo => repo.GetCategories())
                    .Returns(GetTestCategories());
            var controller = new CategoriesController(logger.Object, mockCategoryService.Object);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Category>>(
                viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        private IList<Category> GetTestCategories()
        {
            var categories = new List<Category>();
            categories.Add(new Category()
            {
                CategoryId = 1,
                CategoryName = "Test One"
            });
            categories.Add(new Category()
            {
                CategoryId = 2,
                CategoryName = "Test Two"
            });
            return categories;
        }
    }
}
