using Catalog.BLL.Services;
using Catalog.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;

namespace MyCatalogSite.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly ICategoryService categoryService;
        private readonly IProductService productService;
        private readonly ISupplierService supplierService;

        public ProductsController(ILogger<ProductsController> logger, ICategoryService categoryService, IProductService productService, ISupplierService supplierService)
        {
            _logger = logger;
            this.categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            this.productService = productService ?? throw new ArgumentNullException(nameof(productService));
            this.supplierService = supplierService ?? throw new ArgumentNullException(nameof(supplierService));
        }
        public IActionResult Index()
        {
            var products = productService.GetProducts(0);
            return View(products);
        }

        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            var product = productService.GetProductById(id);

            if (product == null)
            {
                return RedirectToAction("Index");
            }
            SetCategoriesAndSuppliersToViewBag();

            return View(product);
        }

        [HttpPost]
        public IActionResult EditProduct(int id, Product input)
        {
            var product = productService.GetProductById(id);

            if (product != null && ModelState.IsValid)
            {
                productService.UpdateProduct(input);
                return RedirectToAction("Products");
            }
            SetCategoriesAndSuppliersToViewBag();

            return View(product);
        }
        [HttpGet]
        public IActionResult CreateProduct()
        {
            SetCategoriesAndSuppliersToViewBag();

            return View(new Product());
        }


        [HttpPost]
        public IActionResult CreateProduct(Product input)
        {

            if (ModelState.IsValid)
            {
                productService.AddProduct(input);
                return RedirectToAction("Products");
            }
            SetCategoriesAndSuppliersToViewBag();

            return View(input);
        }
        private void SetCategoriesAndSuppliersToViewBag()
        {
            var categories = new SelectList(categoryService.GetCategories(), "CategoryId", "CategoryName");
            var suppliers = new SelectList(supplierService.GetSuppliers(), "SupplierId", "CompanyName");

            ViewBag.Categories = categories;
            ViewBag.Suppliers = suppliers;
        }
    }
}
