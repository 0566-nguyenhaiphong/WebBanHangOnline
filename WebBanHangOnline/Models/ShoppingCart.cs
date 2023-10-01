using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanHangOnline.Models
{
   
    public class ShoppingCart
    {
        public List<ShoppingCartItem> Items { get; set; }
        public ShoppingCart() 
        { 
            this.Items = new List<ShoppingCartItem>();
        }
        public void AddToCart(ShoppingCartItem item , int quantity)
        {
            //check xem item đã tồn tại trong giỏ hàng chưa
            var checkExist = Items.FirstOrDefault(x => x.ProductId == item.ProductId);
            if(checkExist != null)
            {
                checkExist.Quantity += quantity;
                checkExist.PriceTotal = checkExist.Price * checkExist.Quantity;
                
            }
            else
            {
                Items.Add(item);
            }
        }
        public void Remove(int id)
        {
            var checkExist = Items.SingleOrDefault(x => x.ProductId == id);
            if(checkExist != null)
            {
                Items.Remove(checkExist);               
            }
            
        }

        public void UpdateQuantity(int id, int quantity)
        {
            var checkExist = Items.SingleOrDefault(x => x.ProductId == id);
            if (checkExist != null)
            {
                checkExist.Quantity = quantity;
                checkExist.PriceTotal = checkExist.Price * checkExist.Quantity;
            }
        }
        public decimal GetToTalPrice()
        {
            return Items.Sum(x => x.PriceTotal);
        }
        public int GetToTalQuantity()
        {
            return Items.Sum(x => x.Quantity);
        }
        public void ClearCart()
        {
            Items.Clear();
        }
    }
    public class ShoppingCartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Alias { get; set; }

        public string CategoryName { get; set; }

        public string ProductImg { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal PriceTotal { get; set;}
    }

}