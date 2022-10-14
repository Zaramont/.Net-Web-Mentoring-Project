using Catalog.BLL.Services;
using Catalog.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using MySite.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MySite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryService categoryService;
        private readonly IProductService productService;
        private readonly ISupplierService supplierService;

        public HomeController(ILogger<HomeController> logger, ICategoryService categoryService, IProductService productService, ISupplierService supplierService)
        {
            _logger = logger;
            this.categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            this.productService = productService ?? throw new ArgumentNullException(nameof(productService));
            this.supplierService = supplierService ?? throw new ArgumentNullException(nameof(supplierService));
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Categories()
        {
            var categories = categoryService.GetCategories();
            return View(categories);
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
    }
}
