using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();   
        // GET: Product
        public ActionResult Index(int? id, int? page, string alias)
        {
            var pageSize = 8;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Product> items = db.Products.OrderByDescending(x => x.Id);
            if (id != null)
            {
                items = items.Where(x => x.Id == id).ToList();
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        [AllowAnonymous]

        public ActionResult Detail( string alias, int id)
        {
            var item = db.Products.Find(id);
            if(item != null)
            {
                db.Products.Attach(item);
                item.ViewCount = item.ViewCount + 1;
                db.Entry(item).Property(x => x.ViewCount).IsModified = true;
                db.SaveChanges();
            }
            return View(item);
        }
        public ActionResult ProductCategory(int? page, int id)
        {
            var pageSize = 8;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Product> items = db.Products.OrderByDescending(x => x.Id);
            if (id > 0)
            {
                items = items.Where(x => x.ProductCategoryId == id).ToList();
            }
            var cate = db.ProductCategories.Find(id);
            if(cate != null)
            {
                ViewBag.CateName = cate.Title;
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.CateId = id;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        public ActionResult Partial_ItemsByCateId()
        {
            var items = db.Products.Where(x=> x.isHome && x.IsActice).Take(12).ToList();
            return PartialView(items);
        }
        public ActionResult Partial_ProductSale()
        {
            var items = db.Products.Where(x => x.isSale && x.IsActice).Take(12).ToList();
            return PartialView(items);
        }
        
    }
}