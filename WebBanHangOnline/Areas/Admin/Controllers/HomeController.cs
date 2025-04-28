using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Employee")]
    public class HomeController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            AdminDashboardViewModel model = GetDashboardData();
            return View(model);
        }

        private AdminDashboardViewModel GetDashboardData()
        {
            AdminDashboardViewModel dashboard = new AdminDashboardViewModel();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                // Tổng số đơn hàng
                dashboard.TotalOrders = db.Orders.Count();

                // Tổng doanh thu
                dashboard.TotalRevenue = db.Orders.Sum(o => o.TotalAmount);

                // Tổng số người dùng
                dashboard.TotalUsers = db.Users.Count();

                // Tổng số sản phẩm
                dashboard.TotalProducts = db.Products.Count();
            }

            return dashboard;
        }
    }
}