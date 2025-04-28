using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models.EF;
using WebBanHangOnline.Models;

namespace WebBanHangOnline.Controllers
{
    public class VoucherController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index(int? page)
        {
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Voucher> items = db.Vouchers.OrderByDescending(x => x.CreatedDate).Where(x => x.UserName == User.Identity.Name || x.UserName == null);
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        public ActionResult Detail(int id)
        {
            var item = db.Vouchers.Find(id);
            return View(item);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Save(int id)
        {
            if (Request.IsAuthenticated == false)
            {
                return Json(new { Success = false, Message = "Vui lòng đăng nhập để sử dụng chức năng này!" });
            }
            var checkitem = db.Vouchers.FirstOrDefault(x => x.Id == id);

            if (checkitem == null)
            {
                return Json(new { Success = false, Message = "Voucher không tồn tại" });
            }

            checkitem.UserName = User.Identity.Name;
            checkitem.isSave = true;
            db.SaveChanges();

            return Json(new { Success = true, Message = "Đã lưu" });
        }

    }
}