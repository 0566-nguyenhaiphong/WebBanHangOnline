using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class VoucherController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Voucher
        public ActionResult Index(int? page)
        {
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Voucher> items = db.Vouchers.OrderByDescending(x => x.Id);
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
            
        }
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Voucher model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.CreatedBy = User.Identity.Name;
                model.ModifiedDate  = DateTime.Now;
                model.isUse = false;
                // Tạo mã voucher ngẫu nhiên
                model.VoucherCode = GenerateRandomVoucherCode();

                db.Vouchers.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        private string GenerateRandomVoucherCode()
        {
            // Sử dụng một thư viện hoặc hàm tạo ngẫu nhiên để tạo mã
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            // Tạo mã voucher với 6 ký tự viết hoa
            string voucherCode = new string(Enumerable.Repeat(chars, 6)
                                          .Select(s => s[random.Next(s.Length)])
                                          .ToArray());

            // Kiểm tra xem mã voucher đã tồn tại trong hệ thống chưa
            while (db.Vouchers.Any(v => v.VoucherCode == voucherCode))
            {
                voucherCode = new string(Enumerable.Repeat(chars, 6)
                                          .Select(s => s[random.Next(s.Length)])
                                          .ToArray());
            }

            return voucherCode;
        }

        public ActionResult Edit(int id)
        {
            var item = db.Vouchers.Find(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Voucher model)
        {
            if (ModelState.IsValid)
            {
                model.ModifiedDate = DateTime.Now;
                model.ModifiedBy = User.Identity.Name;
                db.Vouchers.Attach(model);
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.Vouchers.Find(id);
            if (item != null)
            {
                db.Vouchers.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}