using Catalog.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MyCatalogSite.Controllers;
using Xunit;

namespace MySiteTestProject.UnitTests.Controllers
{
    public class HomeControllerTests
    {
        [Fact]
        public void Index_ReturnsAViewResult()
        {
            // Arrange
            var logger = new Mock<ILogger<HomeController>>();
            var mockProductService = new Mock<IProductService>();

            var controller = new HomeController(logger.Object, mockProductService.Object);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
        }
    }
}
