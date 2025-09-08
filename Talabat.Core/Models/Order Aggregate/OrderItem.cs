using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Models.Order_Aggregate
{
    public class OrderItem : BaseModel
    {

        private OrderItem()
        {
            
        }
        public OrderItem(ProductItemOrdered product, decimal price, int quantity)
        {
            Product = product;
            Price = price;
            Quantity = quantity;
        }

        public ProductItemOrdered Product { get; set; } = null!;
        public decimal Price { get; set; } // Price of product as a item at the time of order 
        public int Quantity { get; set; }
    }
}
