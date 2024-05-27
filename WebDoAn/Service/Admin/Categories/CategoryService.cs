using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebDoAn.dbs;
using WebDoAn.Models;
using WebDoAn.Service.Admin.Categories.Dto;

namespace WebDoAn.Service.Admin.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly DoAnDbContext _db;
        public INotyfService _notyfService;
        public CategoryService(DoAnDbContext db, INotyfService notyfService) {
            _db = db; 
            _notyfService = notyfService;
        }
        public bool Create(Category category)
        {
            var isCate = _db.categorie.Where(x => x.Name == category.Name).ToList();
            if(isCate == null)
            {
                _db.categorie.Add(category);
                _db.SaveChanges();
                _notyfService.Success("Tạo mới thành công");
                return true;
            }
            else
            {
                _notyfService.Error("Tên loại hàng này đã tồn tại, vui lòng thay đổi tên !");
                return false;
            }
            
        }

        public bool Delete(int id)
        {
            var cate = _db.categorie.Find(id);
            if(cate == null)
            {
                _notyfService.Error("Không tìm thấy loại hàng này");
                return false;
            }
            else
            {
                _db.Remove(cate);
                _db.SaveChanges();
                _notyfService.Success("Xóa thành công");
                return true;
            }
          
        }

        public List<Category> GetAll(GetInput input)
        {
            if(input.Name == "")
            {
                var list = _db.categorie.OrderBy(x => x.Id).ToList();
                return list;
            }
            else
            {
                var list = _db.categorie.Where(x => x.Name.ToLower().Contains(input.Name.ToLower())).OrderBy(x => x.Id).ToList();
                return list;
            }
            
        }

        public async Task<List<Category>> GetCategories()
        {
            var listCate = await _db.categorie.OrderBy(x => x.Id).ToListAsync();
            return listCate;
        }

        public Category GetCategoryById(int id)
        {
            var cate = _db.categorie.Find(id);
            if(cate == null)
            {
                Category category = new Category();
                _notyfService.Error("Không tìm thấy loại hàng này");
                return category;
            }
            else
            {
                return cate;
            }
        }

        public bool Update(Category category)
        {
            _db.Update(category);
            _db.SaveChanges();
            return true;
        }
    }
}
