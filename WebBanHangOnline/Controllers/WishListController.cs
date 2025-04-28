using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Web.UI;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Controllers
{
    public class WishListController : Controller
    {
        // GET: WishList
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(int ? page)
        {
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            IEnumerable<Wishlist> items = db.Wishlists.Where(x => x.UserName == User.Identity.Name).OrderByDescending(x => x.CreatedDate);
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult PostWishList( int ProductId)
        {
            if(Request.IsAuthenticated == false)
            {
                return Json(new { Success = false, Message = "Vui lòng đăng nhập để sử dụng chức năng này!" });
            }
            var checkitem = db.Wishlists.FirstOrDefault(x => x.ProductId == ProductId && x.UserName == User.Identity.Name);
            if(checkitem != null)
            {
                return Json(new { Success = false, Message = "Sản phẩm đã được thêm vào yêu thích rồi" });
            }
            var item = new Wishlist();
            item.ProductId = ProductId;
            item.UserName = User.Identity.Name;
            item.CreatedDate = DateTime.Now;    
            db.Wishlists.Add(item);
            db.SaveChanges();
            return Json(new { Success = true });
        }
        [HttpPost]
        [AllowAnonymous]    
        public ActionResult PostDeleteWishList(int ProductId)
        {
            var checkitem = db.Wishlists.FirstOrDefault(x => x.ProductId == ProductId && x.UserName == User.Identity.Name);
            if (checkitem != null)
            {
                db.Wishlists.Remove(checkitem);
                db.SaveChanges();
                return Json(new { Success = true });
            }
            return Json(new { Success = false});

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}