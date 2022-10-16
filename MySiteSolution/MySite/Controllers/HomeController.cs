using Catalog.BLL.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySite.Models;
using System;
using System.Diagnostics;

namespace MySite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService productService;

        public HomeController(ILogger<HomeController> logger, IProductService productService)
        {
            _logger = logger;
            this.productService = productService ?? throw new ArgumentNullException(nameof(productService));
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
