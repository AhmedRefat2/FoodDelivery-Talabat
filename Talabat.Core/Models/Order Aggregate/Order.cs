using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Models.Order_Aggregate
{
    public class Order : BaseModel
    {

        private Order()
        {

        }

        public Order(string buyerEmail, Address shippingAddress, int? deliveryMethodId, ICollection<OrderItem> items, decimal subtotal)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethodId = deliveryMethodId;
            Items = items;
            Subtotal = subtotal;
        }

        public string BuyerEmail { get; set; } = null!;
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public Address ShippingAddress { get; set; } = null!;
        public int? DeliveryMethodId { get; set; } // Forigen Key 
        public DeliveryMethod? DeliveryMethod { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>(); // Nav Many 
        public decimal Subtotal { get; set; } // cost of order without Shipping cost

        //[NotMapped]
        //public decimal Total => Subtotal + DeliveryMethod.Cost;
        public decimal GetTotal() => Subtotal + DeliveryMethod.Cost;
        public string PaymentIntentId { get; set; } = string.Empty; // For Payment Integration
    }
}
