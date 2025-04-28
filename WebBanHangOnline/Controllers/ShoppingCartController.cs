using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
using PagedList;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WebBanHangOnline.Common;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;
using WebBanHangOnline.Models.Payments;

namespace WebBanHangOnline.Controllers
{
    public class ShoppingCartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private MoMoSecurity momoSecurity = new MoMoSecurity(); // Assuming MoMoSecurity is in the same namespace
        // GET: ShoppingCart
        public ActionResult Index()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                ViewBag.CheckCart = cart;
            }
            return View();
        }
        public ActionResult Partial_Item_Cart()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                ViewBag.CheckCart = cart;
                return PartialView(cart.Items);
                
            }
            return PartialView();
        }

        public ActionResult ShowCount()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if(cart != null && cart.Items.Any())
            {
                return Json(new { Count = cart.Items.Count }, JsonRequestBehavior.AllowGet);

            }
            return Json(new {  Count = 0 }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddToCart(int id, int quantity)
        {
            var code = new { Success = false, msg = "", code = -1, Count = 0 };
           
            var checkProduct = db.Products.FirstOrDefault(x => x.Id == id);
            if (checkProduct != null)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if (cart == null)
                {
                    cart = new ShoppingCart();
                }
                ShoppingCartItem item = new ShoppingCartItem
                {
                    ProductId = checkProduct.Id,
                    ProductName = checkProduct.Title,
                    CategoryName = checkProduct.ProductCategory.Title,   
                    Alias = checkProduct.Alias,
                    Quantity = quantity
                };
                if (checkProduct.ProductImage.FirstOrDefault(x => x.IsDefault) != null)
                {
                    item.ProductImg = checkProduct.ProductImage.FirstOrDefault(x => x.IsDefault).Image;
                }
                item.Price = checkProduct.Price;
                if (checkProduct.PriceSale > 0)
                {
                    item.Price = (decimal)checkProduct.PriceSale;
                }
                item.PriceTotal = item.Quantity * item.Price;
                cart.AddToCart(item, quantity);
                Session["Cart"] = cart;
                code = new { Success = true, msg = "Thêm sản phẩm vào giở hàng thành công!", code = 1, Count = cart.Items.Count };
            }
            return Json(code);
        }
        [HttpPost]
        public ActionResult UpdateCart(int id, int quantity)
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                cart.UpdateQuantity(id, quantity);
                return Json(new { Success = true });
            }
            return Json(new { Success = false });
        }
       
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var code = new { Success = false, msg = "", code = -1, Count = 0 };
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                var checkProduct = cart.Items.FirstOrDefault(x => x.ProductId == id);
                if(checkProduct != null)
                {
                    cart.Remove(id);
                    code = new { Success = true, msg = "Xóa thành công", code = 1, Count = cart.Items.Count };
                    
                }
            }
            return Json(code);
        }
        [HttpPost]
        public ActionResult DeleteAll()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            var code = new { Success = true, code = -1, Count = 0 };
            if (cart != null && cart.Items.Any())
            {
                cart.ClearCart();
                code = new { Success = true, code = 1, Count = cart.Items.Count };
                
            }
            return Json(code);
        }
        public ActionResult Parttial_CheckOut()
        {
            return PartialView();
        }
      
      
        public ActionResult CheckOut()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                ViewBag.CheckCart = cart;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(OrderViewModel req)
        {
            var code = new { success = false, Code = -1, Url = "" };
            if (ModelState.IsValid)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if (cart != null )
                {
                    Order order = new Order();
                    order.CustomerName = req.CustomerName;
                    order.Phone = req.Phone;
                    order.Address = req.Address;
                    order.Email = req.Email;
                    order.Status = 1;//chưa thanh toán / 2/đã thanh toán, 3/Hoàn thành, 4/hủy
                    cart.Items.ForEach(x => order.OrderDetails.Add(new OrderDetail
                    {
                        ProductId = x.ProductId,
                        Quantity = x.Quantity,
                        Price = x.Price
                    }));


                    order.TotalAmount = Math.Max(0, cart.Items.Sum(x => (x.Price * x.Quantity)) - cart.TotalDiscount);
                    order.TypePayment = req.TypePayment;
                    order.CreatedDate = DateTime.Now;
                    order.ModifiedDate = DateTime.Now;
                    order.CreatedBy = User.Identity.GetUserName();
                    if (User.Identity.IsAuthenticated)
                    {
                        order.CustomerId = User.Identity.GetUserId();

                    }
                    Random rd = new Random();
                    order.Code = "DH" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
                    Session["OrderCode"] = order.Code;
                    db.Orders.Add(order);
                    db.SaveChanges();
                    //send mail cho khachs hang
                    var strSanPham = "";
                    var thanhtien = decimal.Zero;
                    var TongTien = decimal.Zero;
                    foreach (var sp in cart.Items)
                    {
                        strSanPham += "<tr>";
                        strSanPham += "<td>" + sp.ProductName + "</td>";
                        strSanPham += "<td>" + sp.Quantity + "</td>";
                        strSanPham += "<td>" + WebBanHangOnline.Models.Common.FormatNumber.FormatNumber1(Convert.ToInt32(sp.PriceTotal)) + "</td>";
                        strSanPham += "</tr>";
                        thanhtien += sp.Price * sp.Quantity;
                    }
                    TongTien = thanhtien - cart.TotalDiscount;
                    string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send2.html"));
                    contentCustomer = contentCustomer.Replace("{{MaDon}}", order.Code);
                    contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
                    contentCustomer = contentCustomer.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                    contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", order.CustomerName);
                    contentCustomer = contentCustomer.Replace("{{Phone}}", order.Phone);
                    contentCustomer = contentCustomer.Replace("{{Email}}", req.Email);
                    contentCustomer = contentCustomer.Replace("{{DiaChiNhanHang}}", order.Address);
                    contentCustomer = contentCustomer.Replace("{{ThanhTien}}", WebBanHangOnline.Models.Common.FormatNumber.FormatNumber1(Convert.ToInt32(thanhtien)));
                    contentCustomer = contentCustomer.Replace("{{TongTien}}", WebBanHangOnline.Models.Common.FormatNumber.FormatNumber1(Convert.ToInt32(TongTien)));
                    WebBanHangOnline.Common.Common.SendMail("ShopOnline", "Đơn hàng #" + order.Code, contentCustomer.ToString(), req.Email);

                    string contentAdmin = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send1.html"));
                    contentAdmin = contentAdmin.Replace("{{MaDon}}", order.Code);
                    contentAdmin = contentAdmin.Replace("{{SanPham}}", strSanPham);
                    contentAdmin = contentAdmin.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                    contentAdmin = contentAdmin.Replace("{{TenKhachHang}}", order.CustomerName);
                    contentAdmin = contentAdmin.Replace("{{Phone}}", order.Phone);
                    contentAdmin = contentAdmin.Replace("{{Email}}", req.Email);
                    contentAdmin = contentAdmin.Replace("{{DiaChiNhanHang}}", order.Address);
                    contentAdmin = contentAdmin.Replace("{{ThanhTien}}", WebBanHangOnline.Models.Common.FormatNumber.FormatNumber1(Convert.ToInt32(thanhtien)));
                    contentAdmin = contentAdmin.Replace("{{TongTien}}", WebBanHangOnline.Models.Common.FormatNumber.FormatNumber1(Convert.ToInt32(TongTien)));
                    WebBanHangOnline.Common.Common.SendMail("ShopOnline", "Đơn hàng mới #" + order.Code, contentAdmin.ToString(), ConfigurationManager.AppSettings["EmailAdmin"]);
                    cart.ClearCart();
                    if(req.TypePayment == 1) //COD
                    {
                        order.Status = 2;
                        
                        db.SaveChanges();
                        code = new { success = true, Code = req.TypePayment, Url = "" };

                    }
                    
                    if (req.TypePayment == 2) //vnpay
                    {
                        var url = UrlPayment(req.TypePaymentVN, order.Code);
                        code = new { success = true, Code = req.TypePayment, Url = url };
                    }
                    else if (req.TypePayment == 3) // Momo
                    {
                        var url = UrlPaymentMomo(order.Code);
                        code = new { success = true, Code = req.TypePayment, Url = url };
                    }
                    //return RedirectToAction("CheckOutSuccess");

                }

            }
            return Json(code);
        }
        public ActionResult VnpayReturn()
        {
            if (Request.QueryString.Count > 0)
            {
                string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat
                var vnpayData = Request.QueryString;
                VnPayLibrary vnpay = new VnPayLibrary();

                foreach (string s in vnpayData)
                {
                    //get all querystring data
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s, vnpayData[s]);
                    }
                }
                string orderCode = Convert.ToString(vnpay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                String vnp_SecureHash = Request.QueryString["vnp_SecureHash"];
                String TerminalID = Request.QueryString["vnp_TmnCode"];
                long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
                String bankCode = Request.QueryString["vnp_BankCode"];

                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {
                        var itemOrder = db.Orders.FirstOrDefault(x => x.Code == orderCode);
                        if (itemOrder != null)
                        {
                            itemOrder.Status = 2;//đã thanh toán
                            db.Orders.Attach(itemOrder);
                            db.Entry(itemOrder).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        //Thanh toan thanh cong
                        ViewBag.InnerText = "Giao dịch được thực hiện thành công. Cảm ơn quý khách đã sử dụng dịch vụ";
                        //log.InfoFormat("Thanh toan thanh cong, OrderId={0}, VNPAY TranId={1}", orderId, vnpayTranId);
                    }
                    else
                    {
                        //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                        ViewBag.InnerText = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;
                        //log.InfoFormat("Thanh toan loi, OrderId={0}, VNPAY TranId={1},ResponseCode={2}", orderId, vnpayTranId, vnp_ResponseCode);
                    }
                    //displayTmnCode.InnerText = "Mã Website (Terminal ID):" + TerminalID;
                    //displayTxnRef.InnerText = "Mã giao dịch thanh toán:" + orderId.ToString();
                    //displayVnpayTranNo.InnerText = "Mã giao dịch tại VNPAY:" + vnpayTranId.ToString();
                    ViewBag.ThanhToanThanhCong = "Số tiền thanh toán (VND):" + vnp_Amount.ToString();
                    //displayBankCode.InnerText = "Ngân hàng thanh toán:" + bankCode;
                }
            }
            //var a = UrlPayment(0, "DH3574");
            return View();
        }
        #region Thanh toán vnpay
        public string UrlPayment(int TypePaymentVN, string orderCode)
        {
            var urlPayment = "";
            var order = db.Orders.FirstOrDefault(x => x.Code == orderCode);
            //Get Config Info
            string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Secret Key

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();
            var Price = (long)order.TotalAmount * 100;
            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", Price.ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            if (TypePaymentVN == 1)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
            }
            else if (TypePaymentVN == 2)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNBANK");
            }
            else if (TypePaymentVN == 3)
            {
                vnpay.AddRequestData("vnp_BankCode", "INTCARD");
            }

            vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán đơn hàng :" + order.Code);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", order.Code); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            //Add Params of 2.1.0 Version
            //Billing

            urlPayment = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            //log.InfoFormat("VNPAY URL: {0}", paymentUrl);
            return urlPayment;
        }
        #endregion
        public ActionResult MomoReturn()
        {
                string orderId = (string)Session["OrderCode"];
                // Xử lý kết quả thanh toán ở đây
                if ( orderId!=null)
                {
                    // Thanh toán thành công, cập nhật trạng thái đơn hàng
                    var itemOrder = db.Orders.FirstOrDefault(x => x.Code == orderId);
                    if (itemOrder != null)
                    {
                        itemOrder.Status = 2; // Đã thanh toán
                        db.Orders.Attach(itemOrder);
                        db.Entry(itemOrder).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }

                    ViewBag.InnerText = "Giao dịch được thực hiện thành công. Cảm ơn quý khách đã sử dụng dịch vụ";
                }
                else
                {
                // Xử lý trường hợp thanh toán không thành công
                ViewBag.InnerText = "Có lỗi xảy ra trong quá trình xử lý.";
                }
            
            
            

            return View();
        }


        #region Thanh toán Momo
        public string UrlPaymentMomo(string orderCode)
        {
            
            var order = db.Orders.FirstOrDefault(x => x.Code == orderCode);

            string endpoint = ConfigurationManager.AppSettings["MomoEndpoint"];
            string partnerCode = ConfigurationManager.AppSettings["MomoPartnerCode"];
            string accessKey = ConfigurationManager.AppSettings["MomoAccessKey"];
            string secretKey = ConfigurationManager.AppSettings["MomoSecretKey"];
            string orderInfo = "Thanh toán đơn hàng";
            string returnUrl = "https://localhost:44378/ShoppingCart/MomoReturn";
            string notifyUrl = "http://alatoi-001-site1.gtempurl.com/ShoppingCart/CheckOutSuccess"; //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test
            decimal totalAmount = order.TotalAmount;
            string amount = (totalAmount).ToString("0"); // Convert to string with no decimal point
            string orderId = order.Code;
            string requestId = DateTime.Now.Ticks.ToString();
            string extraData = "";

            // Before sign HMAC SHA256 signature
            string rawHash = $"partnerCode={partnerCode}&accessKey={accessKey}&requestId={requestId}&amount={amount}&orderId={orderId}&orderInfo={orderInfo}&returnUrl={returnUrl}&notifyUrl={notifyUrl}&extraData={extraData}";

            MoMoSecurity crypto = new MoMoSecurity();
            // Sign signature SHA256
            string signature = crypto.signSHA256(rawHash, secretKey);

            // Build body json request
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderId },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyUrl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }
            };

            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

            JObject jObject = JObject.Parse(responseFromMomo);
            string payUrl = (string)jObject["payUrl"];
            return payUrl;
        }
        #endregion

        public ActionResult Partial_Item_ThanhToan()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                // Lấy thông tin về giảm giá đã áp dụng từ Session (có thể được cập nhật bởi hàm ApplyVoucher)
                var totalDiscountPrice = Session["TotalDiscountPrice"] as decimal? ?? 0;
                var totalDiscountPercent = Session["TotalDiscountPercent"] as decimal? ?? 0;
                // Trừ giảm giá tổng cộng từ tổng tiền của giỏ hàng
                cart.TotalDiscount = totalDiscountPrice + totalDiscountPercent;
               
                cart.TotalAmount = cart.Items.Sum(x => (x.Price * x.Quantity));
                Session["TotalAmount"] = cart.TotalAmount;
                // Trả về PartialView với danh sách sản phẩm đã cập nhật giảm giá
                return PartialView(cart.Items);
            }

            // Nếu giỏ hàng không có sản phẩm, trả về PartialView trống
            return PartialView();
        }


        [HttpPost]
        public ActionResult ApplyVoucher(string ids)
        {
            try
            {
                // Convert danh sách ID voucher từ string sang List<int>
                var voucherIds = ids.Split(',').Select(int.Parse).ToList();

                // Lặp qua danh sách voucher và cập nhật giá trị isApply
                foreach (var voucherId in voucherIds)
                {
                    var voucher = db.Vouchers.FirstOrDefault(v => v.Id == voucherId);
                    if (voucher != null)
                    {
                        voucher.isApply = true;
                        
                    }
                }
                var TotalAmount = Session["TotalAmount"] as decimal? ?? 0;
                // Lưu tổng giảm giá vào Session để sử dụng trong Partial_Item_ThanhToan
                decimal totalDiscountPrice = db.Vouchers.Where(v => voucherIds.Contains(v.Id)).Sum(v => v.ReducePrice) ?? 0;
                decimal totalDiscountPercent = db.Vouchers.Where(v => voucherIds.Contains(v.Id)).Sum(v => v.PercentReduce * TotalAmount/100) ??0;
                Session["TotalDiscountPercent"] = totalDiscountPercent;
                Session["TotalDiscountPrice"] = totalDiscountPrice;

                db.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
        [HttpPost]
        public ActionResult UpdateVoucherStatus(int voucherId, bool isApply)
        {
            try
            {
                var voucher = db.Vouchers.FirstOrDefault(v => v.Id == voucherId);
                if (voucher != null)
                {
                    voucher.isApply = isApply;
                    db.SaveChanges();

                    return Json(new { success = true });
                }

                return Json(new { success = false, error = "Không tìm thấy voucher." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult ClearTotalDiscount()
        {
            Session.Remove("TotalDiscountPrice");
            Session.Remove("TotalDiscountPercent");

            return Json(new { success = true });
        }



        public ActionResult ListVoucher()
        {

            IEnumerable<Voucher> items = db.Vouchers.OrderByDescending(x => x.CreatedDate).Where(x => x.UserName == User.Identity.Name);

            return View(items);

        }



        public ActionResult CheckOutSuccess()
        {
            return View();
        }
        public ActionResult OrderList()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var items = db.Orders.Where(x => x.CustomerId == userId && x.Status == 2).ToList(); // Chuyển kết quả thành danh sách

                return View(items);
            }
            return View();
           
        }
        public ActionResult DetailOrder(int? id)
        {
            var items = db.OrderDetails.Where(d => d.OrderId == id).ToList();
            return View(items);
        }
      

    }
}