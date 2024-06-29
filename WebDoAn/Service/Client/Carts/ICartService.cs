using WebDoAn.Models;

namespace WebDoAn.Service.Client.Carts
{
    public interface ICartService
    {
        public Task<List<Cart>> GetCart(int id);
        public Task<bool> AddToCart(Cart cart);
        public Task<bool> Update(Cart cart);
        public Task<bool> Delete(int id);

        public Task<bool> CreToCart(Cart cart);
        public Task<bool> DesToCart(Cart cart);

        public Task<bool> UpdateMultil(Cart cart);
    }
}
