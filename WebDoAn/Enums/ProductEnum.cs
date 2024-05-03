using System.ComponentModel.DataAnnotations;

namespace WebDoAn.Enums
{
    public class ProductEnum
    {
        public enum TypeProduct : byte
        {
            [Display(Name = "Đang bán")]
            Active = 0,
            [Display(Name = "Sản phẩm ngừng bán")]
            Stop = 1,
            [Display(Name = "Sản phẩm đã bị xóa")]
            Delete = 2,

        }
    }
}
