using Catalog.DAL.Entities;
using Catalog.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Catalog.BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private ICategoryRepository categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public IList<Category> GetCategories()
        {
            var categories = categoryRepository.GetAll().ToList();

            return categories;
        }

        public Category GetCategoryById(int id)
        {
            var categoryFromDb = categoryRepository.GetById(id);
            return categoryFromDb;
        }

        public void AddCategory(Category entity)
        {
            categoryRepository.Create(entity);
        }

        public void DeleteCategory(int id)
        {
            categoryRepository.Delete(id);
        }

        public void UpdateCategory(Category entity)
        {
            categoryRepository.Update(entity);
        }
    }
}
