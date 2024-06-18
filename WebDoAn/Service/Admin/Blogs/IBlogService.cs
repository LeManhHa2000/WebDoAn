using WebDoAn.Models;
using WebDoAn.Service.Admin.Blogs.Dto;

namespace WebDoAn.Service.Admin.Blogs
{
    public interface IBlogService
    {
        public Task<List<Blog>> GetBlogs();
        public List<Blog> GetAll(GetInput input);
        public Task<Blog> GetBlogById(int id);
        public Task<bool> Create(Blog blog);
        public Task<bool> Update(Blog blog);
        public Task<bool> Delete(int id);
    }
}
