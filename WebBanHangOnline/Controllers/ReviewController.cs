using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Controllers
{
    [Authorize] 
    public class ReviewController : Controller
    {
        // GET: Review
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult _Review(int productId)
        {
            ViewBag.ProductId = productId;  
            var item = new ReviewProduct();
            if (User.Identity.IsAuthenticated)
            {
                var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var userManger = new UserManager<ApplicationUser>(userStore);
                var user = userManger.FindByName(User.Identity.Name);
                if(user != null)
                {
                    item.Email = user.Email;
                    item.FullName = user.Fullname;
                    item.UserName = user.UserName;  
                }
                return PartialView();

            }
            return PartialView();
        }
        [AllowAnonymous]
        public ActionResult _Load_Review(int  productId)
        {
            var item = db.Review.Where(x => x.ProductId == productId).OrderByDescending(x => x.Id).ToList();
            ViewBag.Count = item.Count;
            return PartialView(item);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult PostReview(ReviewProduct req)
        {
            if (ModelState.IsValid)
            {
                req.CreateDate = DateTime.Now;  
                db.Review.Add(req);
                db.SaveChanges();
                return Json(new { Success = true});  
            }
            return Json(new { Success = false });

        }
    }
}