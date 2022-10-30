using Catalog.BLL.Services;
using Catalog.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyCatalogSite.Models;
using System;
using System.IO;
using System.Linq;

namespace MyCatalogSite.Controllers
{
    public class CategoriesController : Controller
    {
        byte skipBytes = 78;
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

        [HttpGet]
        public IActionResult Image(int id)
        {
            var category = categoryService.GetCategoryById(id);
            var image = category.Picture.Skip(skipBytes).ToArray();
            //System.IO.File.WriteAllBytes(Environment.CurrentDirectory + "/" + id+".bmp", image);
            return base.File(image, "image/bmp");
        }

        [HttpGet]
        public IActionResult UploadImage(int id)
        {
            var category = categoryService.GetCategoryById(id);
            if (category == null)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost]
        public IActionResult UploadImage(int id, FileUploadModel model)
        {
            var categoryFromService = categoryService.GetCategoryById(id);
            if (categoryFromService == null || !ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            var file = model?.FormFile;

            if (file.Length <= 15360)
            {
                using (var stream = new MemoryStream())
                {
                    stream.Position = skipBytes;
                    file.CopyTo(stream);
                    categoryFromService.Picture = stream.ToArray();
                    categoryService.UpdateCategory(categoryFromService);
                    return RedirectToAction("Index");
                }
            }

            return View();
        }
    }
}
