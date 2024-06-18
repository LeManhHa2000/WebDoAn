using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.EntityFrameworkCore;
using WebDoAn.dbs;
using WebDoAn.Models;
using WebDoAn.Service.Admin.Blogs.Dto;

namespace WebDoAn.Service.Admin.Blogs
{
    public class BlogService : IBlogService
    {
        private readonly DoAnDbContext _db;
        public INotyfService _notyfService;

        public BlogService(DoAnDbContext db, INotyfService notyfService)
        {
            _db = db;
            _notyfService = notyfService;
        }

        public async Task<bool> Create(Blog blog)
        {
            var isBlog = _db.blog.Where(x => x.Title.ToLower() == blog.Title.ToLower()).ToList();
            if (isBlog.Count() == 0)
            {
                var date = DateTime.Now;
                var datecustom = DateTime.SpecifyKind(date, DateTimeKind.Utc);
                blog.CreateTime = datecustom;

                _db.blog.Add(blog);
                await _db.SaveChangesAsync();
                _notyfService.Success("Tạo mới thành công");
                return true;
            }
            else
            {
                _notyfService.Error("Tiêu đề này đã tồn tại, vui lòng thay đổi tiêu đề !");
                return false;
            }
        }

        public async Task<bool> Delete(int id)
        {
            var blog = _db.blog.Find(id);
            if (blog == null)
            {
                _notyfService.Error("Không tìm thấy loại loại này");
                return false;
            }
            else
            {
                _db.blog.Remove(blog);
                await _db.SaveChangesAsync();
                _notyfService.Success("Xóa thành công");
                return true;
            }
        }

        public List<Blog> GetAll(GetInput input)
        {
            if (input.Name == "all")
            {
                var list = _db.blog.OrderBy(x => x.Id).ToList();
                return list;
            }
            else
            {
                var list = _db.blog.Where(x => x.Title.ToLower().Contains(input.Name.ToLower())).OrderBy(x => x.Id).ToList();
                return list;
            }
        }

        public async Task<Blog> GetBlogById(int id)
        {
            var bl = await _db.blog.FindAsync(id);
            if (bl == null)
            {
                Blog blog = new Blog();
                _notyfService.Error("Không tìm thấy loại sản phẩm này");
                return blog;
            }
            else
            {
                return bl;
            }
           
        }

        public async Task<List<Blog>> GetBlogs()
        {
            var blog = await _db.blog.OrderBy(x => x.Id).ToListAsync();
            return blog;
        }

        public async Task<bool> Update(Blog blog)
        {
            var isBlog = _db.blog.Where(x => blog.Id != x.Id && x.Title.ToLower() == blog.Title.ToLower()).ToList();
            if (isBlog.Count == 0)
            {
                DateTime cate = _db.blog.Where(x => x.Id == blog.Id).Select(x => x.CreateTime).FirstOrDefault();
                blog.CreateTime = cate;
                _db.blog.Update(blog);
                await _db.SaveChangesAsync();
                _notyfService.Success("Cập nhật thành công");
                return true;
            }
            else
            {
                _notyfService.Error("Tiêu đề này đã tồn tại, vui lòng thay đổi tiêu đề !");
                return false;
            }
        }
    }
}
