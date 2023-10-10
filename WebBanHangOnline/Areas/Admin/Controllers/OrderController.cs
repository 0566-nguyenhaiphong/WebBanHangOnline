using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Order     
        public ActionResult Index(int? page)
        {
            var items = db.Orders.OrderByDescending(x => x.CreatedDate).ToList();
            if(page == null)
            {
                page = 1;
            }
            var pageNumber = page ?? 1;
            var pageSize = 15;
            return View(items.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult View(int id)
        {
            var item = db.Orders.Find(id);
            return View(item);
        }
        public ActionResult Partial_SanPham(int id)
        {
            var items = db.OrderDetails.Where(x => x.OrderId == id).ToList();
            return PartialView(items);
        }
        [HttpPost]
        public ActionResult UpdateTT(int id, bool trangthai)
        {
            var item = db.Orders.Find(id);
            if (item != null)
            {
                db.Orders.Attach(item);
                item.isConfirm = trangthai;
                db.Entry(item).Property(x => x.isConfirm).IsModified = true;
                db.SaveChanges();
                return Json(new { message = "Success", Success = true });
            }
            return Json(new { message = "Unsuccess", Success = false });
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.Orders.Find(id);
            if (item != null)
            {
                db.Orders.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        [HttpPost]
        public ActionResult DeleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        var obj = db.Orders.Find(Convert.ToInt32(item));
                        db.Orders.Remove(obj);
                        db.SaveChanges();
                    }
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false });
        }

    }
}