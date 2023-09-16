using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class ProductImageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/ProductImages
        public ActionResult Index(int id)
        {
            ViewBag.ProductId = id;
            var items = db.ProductImages.Where(x=>x.ProductId == id).ToList();
            return View(items);
        }
        [HttpPost]
        public ActionResult AddImage(int productid, string url)
        {
            db.ProductImages.Add(new ProductImage
            {
                ProductId = productid,
                Image = url,
                IsDefault = false
            });
            db.SaveChanges();
            return Json(new { success = true });
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.ProductImages.Find(id);
            if (item != null)
            {
                db.ProductImages.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        [HttpPost]
        public ActionResult SetDefault(int id)
        {
            // Lấy ảnh đang được chọn
            var image = db.ProductImages.FirstOrDefault(p => p.Id == id);
            if (image != null)
            {
                // Đặt ảnh đại diện cho sản phẩm
                image.IsDefault = true;
                //  Đặt tất cả các ảnh khác thành không phải ảnh đại diện
                var otherImages = db.ProductImages.Where(p => p.ProductId == image.ProductId && p.Id != image.Id);
                foreach (var otherImage in otherImages)
                {
                    otherImage.IsDefault = false;
                }
                // Lưu thay đổi vào cơ sở dữ liệu
                db.SaveChanges();

                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }
        [HttpPost]
        public ActionResult DeleteAll(int productId)
        {
            // Kiểm tra xem có ảnh đại diện không
            var defaultImageExist = db.ProductImages.Any(p => p.ProductId == productId);
            if(defaultImageExist)
            { 
                // Xóa các ảnh của sản phẩm được chỉ định
                db.ProductImages.RemoveRange(db.ProductImages.Where(p => p.ProductId == productId && !p.IsDefault));
                // Lưu thay đổi vào cơ sở dữ liệu
                db.SaveChanges();

                return Json(new { success = true, message = "Xóa ảnh sản phẩm thành công!" });
               
            }
            return Json(new { success = false, message = "Không có ảnh để xóa" });
        }




    }
}