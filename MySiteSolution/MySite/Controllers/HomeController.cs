using Catalog.BLL.Services;
using Catalog.DAL.Entities;
using Microsoft.AspNetCore.Diagnostics;
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

        public IActionResult GenerateError()
        {
            productService.GetProductById(-11);
            return StatusCode(500);
        }        
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var exception =
                HttpContext.Features.Get<IExceptionHandlerPathFeature>().Error;

            var errorModel = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            errorModel.ExceptionMessage = exception.Message;

            _logger.LogError($"Request ID: {errorModel.RequestId}{Environment.NewLine}{errorModel.ExceptionMessage} : {exception.StackTrace}");
            return View(errorModel);
        }
        
    }
}
