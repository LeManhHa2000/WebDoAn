using WebDoAn.ModelPrivew;
using WebDoAn.Models;
using WebDoAn.Service.Admin.Products.Dto;


namespace WebDoAn.Service.Admin.Products
{
    public interface IProductService
    {
        public Task<List<Product>> GetProducts();
        public List<Product> GetAll(GetInput input);
        public Task<Product> GetProductById(int id);
        public Task<bool> Create(Product product);
        public Task<bool> Update(Product product);
        public Task<bool> Delete(int id);
        public Task<string> GetNameCategory(int cateId);
        public Task<ProductViewModal> GetProductPriViewById(int id);


    }
}
