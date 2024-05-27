using WebDoAn.Models;
using WebDoAn.Service.Admin.Categories.Dto;

namespace WebDoAn.Service.Admin.Categories
{
    public interface ICategoryService
    {
        public Task<List<Category>> GetCategories();
        public List<Category> GetAll(GetInput input);
        public Category GetCategoryById(int id);
        public bool Create(Category category);
        public bool Update(Category category);
        public bool Delete(int id);
    }
}
