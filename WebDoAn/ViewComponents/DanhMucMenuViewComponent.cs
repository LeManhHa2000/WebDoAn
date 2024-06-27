using Microsoft.AspNetCore.Mvc;
using WebDoAn.Service.Admin.Categories;

namespace WebDoAn.ViewComponents
{
    public class DanhMucMenuViewComponent : ViewComponent
    {
        private readonly ICategoryService _danhmuc;

        public DanhMucMenuViewComponent(ICategoryService danhmuc)
        {
            _danhmuc = danhmuc;
        }

        public IViewComponentResult Invoke()
        {
            var danhmuc = _danhmuc.GetAllCate().OrderBy(x => x.Id);
            return View(danhmuc);
        }
    }
}
