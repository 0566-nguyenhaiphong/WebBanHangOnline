using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;

namespace WebBanHangOnline.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();   
        // GET: Product
        public ActionResult Index()
        {
            return View();
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