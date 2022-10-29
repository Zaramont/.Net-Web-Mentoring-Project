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
        /*
        [Fact]
        public void  IndexPost_ReturnsBadRequestResult_WhenModelStateIsInvalid()
        {
            // Arrange
            var mockRepo = new Mock<IBrainstormSessionRepository>();
            mockRepo.Setup(repo => repo.List())
                .Returns(GetTestSessions());
            var controller = new CategoriesController(mockRepo.Object);
            controller.ModelState.AddModelError("SessionName", "Required");
            var newSession = new CategoriesController.NewSessionModel();

            // Act
            var result =  controller.Index(newSession);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public   IndexPost_ReturnsARedirectAndAddsSession_WhenModelStateIsValid()
        {
            // Arrange
            var mockRepo = new Mock<IBrainstormSessionRepository>();
            mockRepo.Setup(repo => repo.Add(It.IsAny<BrainstormSession>()))
                .Returns(.Completed)
                .Verifiable();
            var controller = new CategoriesController(mockRepo.Object);
            var newSession = new CategoriesController.NewSessionModel()
            {
                SessionName = "Test Name"
            };

            // Act
            var result =  controller.Index(newSession);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            mockRepo.Verify();
        }

        private List<BrainstormSession> GetTestSessions()
        {
            var sessions = new List<BrainstormSession>();
            sessions.Add(new BrainstormSession()
            {
                DateCreated = new DateTime(2016, 7, 2),
                Id = 1,
                Name = "Test One"
            });
            sessions.Add(new BrainstormSession()
            {
                DateCreated = new DateTime(2016, 7, 1),
                Id = 2,
                Name = "Test Two"
            });
            return sessions;
        }*/
    }
}
