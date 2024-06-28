using WebDoAn.Models;

namespace WebDoAn.ModelPrivew
{
    public class OrderViewModal
    {
        public List<CartViewModal> cartViewModals = new List<CartViewModal>();
        public User User { get; set; }
    }
}
