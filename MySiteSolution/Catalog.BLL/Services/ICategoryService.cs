using Catalog.DAL.Entities;
using System.Collections.Generic;

namespace Catalog.BLL.Services
{
    public interface ICategoryService
    {
        IList<Category> GetCategories();
        Category GetCategoryById(int id);
        Category AddCategory(Category entity);
        void DeleteCategory(int id);
        void UpdateCategory(Category entity);
    }
}
