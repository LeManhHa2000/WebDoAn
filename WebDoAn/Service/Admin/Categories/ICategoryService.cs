using WebDoAn.Models;
using WebDoAn.Service.Admin.Categories.Dto;

namespace WebDoAn.Service.Admin.Categories
{
    public interface ICategoryService
    {
        public Task<List<Category>> GetCategories();
        public List<Category> GetAll(GetInput input);
        public Task<Category> GetCategoryById(int id);
        public Task<bool> Create(Category category);
        public Task<bool> Update(Category category);
        public Task<bool> Delete(int id);
    }
}
