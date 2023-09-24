using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Category
        public ActionResult Index()
        {
            var items = db.Categories;
            return View(items);
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Category model)
        {
            if(ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;  
                model.ModifiedDate = DateTime.Now;
                model.Alias = WebBanHangOnline.Models.Common.Filter.FilterChar(model.Title);
                db.Categories.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public ActionResult Edit(int id)
        {
            var items = db.Categories.Find(id);
            return View(items);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category model)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Attach(model);    
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Alias = WebBanHangOnline.Models.Common.Filter.FilterChar(model.Title);
                db.Entry(model).Property(e => e.Title).IsModified = true;
                db.Entry(model).Property(e => e.Alias).IsModified = true;
                db.Entry(model).Property(e => e.Description).IsModified = true;
                db.Entry(model).Property(e => e.SeoDescription).IsModified = true;
                db.Entry(model).Property(e => e.SeoKeywords).IsModified = true;
                db.Entry(model).Property(e => e.SeoTitle).IsModified = true;
                db.Entry(model).Property(e => e.Position).IsModified = true;
                db.Entry(model).Property(e => e.ModifiedBy).IsModified = true;
                db.Entry(model).Property(e => e.ModifiedDate).IsModified = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.Categories.Find(id);
            if(item != null)
            {             
                db.Categories.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }     
    }
}