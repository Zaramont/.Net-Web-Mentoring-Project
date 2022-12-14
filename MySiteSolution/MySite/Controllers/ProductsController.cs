using Catalog.BLL.Services;
using Catalog.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyCatalogSite.Filters;

namespace MyCatalogSite.Controllers
{
    [ServiceFilter(typeof(LogActionFilter))]
    public class ProductsController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IProductService productService;
        private readonly ISupplierService supplierService;
        private int maxAmountOfProductsOnPage = 25;

        public ProductsController(ICategoryService categoryService, IProductService productService, ISupplierService supplierService)
        {
            this.categoryService = categoryService;
            this.productService = productService;
            this.supplierService = supplierService;
        }
        public IActionResult Index()
        {
            var products = productService.GetProducts(maxAmountOfProductsOnPage);
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
                return RedirectToAction("Index");
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

        public IActionResult GenerateError()
        {
            productService.GetProductById(-11);
            return StatusCode(500);
        }

        [HttpPost]
        public IActionResult CreateProduct(Product input)
        {

            if (ModelState.IsValid)
            {
                productService.AddProduct(input);
                return RedirectToAction("Index");
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
