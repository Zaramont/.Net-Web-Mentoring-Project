using Catalog.BLL.Models;
using Catalog.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySiteRESTApi.Controllers
{
    [Route("api/v1/categories")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class CategoriesApiController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesApiController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            string imageUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}/image/";
            var categories = _categoryService.GetCategories()
                .Select(x => new Category
                {
                    CategoryId = x.CategoryId,
                    CategoryName = x.CategoryName,
                    Description = x.Description,
                    Picture = $"{imageUrl}{x.CategoryId}",
                });
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = _categoryService.GetCategoryById(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpGet("image/{id}")]
        public async Task<ActionResult<byte[]>> GetCategoryImage(int id)
        {
            var category = _categoryService.GetCategoryById(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category.Picture);
        }

        [HttpPost("image/{id}")]
        public async Task<ActionResult<Category>> PostCategoryImage(int id, [FromBody]byte[] image)
        {
            var categoryForUpdate = _categoryService.GetCategoryById(id);
            if (categoryForUpdate == null)
            {
                return NotFound();
            }
            categoryForUpdate.Picture = image;
            _categoryService.UpdateCategory(categoryForUpdate);
            return Ok(categoryForUpdate);
        }
    }
}
