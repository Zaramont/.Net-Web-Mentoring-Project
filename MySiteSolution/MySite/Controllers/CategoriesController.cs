using Catalog.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace MyCatalogSite.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly ICategoryService categoryService;

        public CategoriesController(ILogger<CategoriesController> logger, ICategoryService categoryService)
        {
            _logger = logger;
            this.categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        }
        public IActionResult Index()
        {
            var categories = categoryService.GetCategories();
            return View(categories);
        }
    }
}
