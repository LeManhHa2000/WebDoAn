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
        public async Task<bool> Create(Category category)
        {
            var isCate = _db.categorie.Where(x => x.Name.ToLower() == category.Name.ToLower()).ToList();
            if(isCate.Count() == 0)
            {
                var date = DateTime.Now;
                var datecustom = DateTime.SpecifyKind(date, DateTimeKind.Utc);
                category.CreateTime = datecustom;

                _db.categorie.Add(category);
                await _db.SaveChangesAsync();
                _notyfService.Success("Tạo mới thành công");
                return true;
            }
            else
            {
                _notyfService.Error("Tên loại sản phẩm này đã tồn tại, vui lòng thay đổi tên !");
                return false;
            }
            
        }

        public async Task<bool> Delete(int id)
        {
            var cate = _db.categorie.Find(id);
            if(cate == null)
            {
                _notyfService.Error("Không tìm thấy loại loại này");
                return false;
            }
            else
            {
                _db.categorie.Remove(cate);
                await _db.SaveChangesAsync();
                _notyfService.Success("Xóa thành công");
                return true;
            }
          
        }

        public List<Category> GetAll(GetInput input)
        {
            if(input.Name == "all")
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

        public async Task<Category> GetCategoryById(int id)
        {
            var cate = await _db.categorie.FindAsync(id);
            if(cate == null)
            {
                Category category = new Category();
                _notyfService.Error("Không tìm thấy loại sản phẩm này");
                return category;
            }
            else
            {
                return cate;
            }
        }

        public async Task<bool> Update(Category category)
        {
            var isCate = _db.categorie.Where(x => category.Id != x.Id && x.Name.ToLower() == category.Name.ToLower()).ToList();
            if(isCate.Count == 0)
            {
                DateTime cate = _db.categorie.Where(x => x.Id == category.Id).Select(x => x.CreateTime).FirstOrDefault();

				// Tao moi ngay cập nhật
				var date = DateTime.Now;
				var datecustom = DateTime.SpecifyKind(date, DateTimeKind.Utc);
				category.UpdateTime = datecustom;

				category.CreateTime = cate;
                _db.Update(category);
                await _db.SaveChangesAsync();
                _notyfService.Success("Cập nhật thành công");
                return true;
            }
            else
            {
                _notyfService.Error("Tên loại sản phẩm này đã tồn tại, vui lòng thay đổi tên !");
                return false;
            }

        }
    }
}
