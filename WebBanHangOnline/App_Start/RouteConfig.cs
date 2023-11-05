using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebBanHangOnline
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
               name: "Contact",
               url: "lien-he",
               defaults: new { controller = "Contact", action = "Index", alias = UrlParameter.Optional },
               namespaces: new[] { "WebBanHangOnline.Controllers" }
            );
            routes.MapRoute(
               name: "Product",
               url: "san-pham",
               defaults: new { controller = "Product", action = "Index", alias = UrlParameter.Optional },
               namespaces: new[] { "WebBanHangOnline.Controllers" }
           );
            routes.MapRoute(
               name: "CheckOut",
               url: "thanh-toan",
               defaults: new { controller = "ShoppingCart", action = "CheckOut", alias = UrlParameter.Optional },
               namespaces: new[] { "WebBanHangOnline.Controllers" }
            );
            routes.MapRoute(
            name: "vnpay_return",
            url: "vnpay_return",
            defaults: new { controller = "ShoppingCart", action = "VnpayReturn", alias = UrlParameter.Optional },
            namespaces: new[] { "WebBanHangOnline.Controllers" }
            );
            routes.MapRoute(
              name: "ShoppingCart",
              url: "gio-hang",
              defaults: new { controller = "ShoppingCart", action = "Index", alias = UrlParameter.Optional },
              namespaces: new[] { "WebBanHangOnline.Controllers" }
           );
            routes.MapRoute(
               name: "OrderList",
               url: "don-hang",
               defaults: new { controller = "ShoppingCart", action = "OrderList", alias = UrlParameter.Optional },
               namespaces: new[] { "WebBanHangOnline.Controllers" }
           );
            routes.MapRoute(
               name: "DetailOrder",
               url: "{chi-tiet-don-hang}-p{id}",
               defaults: new { controller = "ShoppingCart", action = "DetailOrder", id = UrlParameter.Optional },
               namespaces: new[] { "WebBanHangOnline.Controllers" }
              );
            routes.MapRoute(
                name: "ProductCategory",
                url: "danh-muc-san-pham/{alias}-{id}",
                defaults: new { controller = "Product", action = "ProductCategory", id = UrlParameter.Optional },
                namespaces: new[] { "WebBanHangOnline.Controllers" }
            );
            routes.MapRoute(
               name: "DetailProduct",
               url: "chi-tiet/{alias}-p{id}",
               defaults: new { controller = "Product", action = "Detail", alias = UrlParameter.Optional },
               namespaces: new[] { "WebBanHangOnline.Controllers" }
           );
           
            routes.MapRoute(
            name: "BaiViet",
            url: "post/{alias}",
            defaults: new { controller = "Article", action = "Index", alias = UrlParameter.Optional },
            namespaces: new[] { "WebBanHangOnline.Controllers" }
            );
            routes.MapRoute(
            name: "DetailNew",
            url: "{alias}-n{id}",
            defaults: new { controller = "News", action = "Detail", id = UrlParameter.Optional },
            namespaces: new[] { "WebBanHangOnline.Controllers" }
           );
            routes.MapRoute(
             name: "NewsList",
             url: "tin-tuc",
             defaults: new { controller = "News", action = "Index", alias = UrlParameter.Optional },
             namespaces: new[] { "WebBanHangOnline.Controllers" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] {"WebBanHangOnline.Controllers"} 
            );
           
        }
    }
}
