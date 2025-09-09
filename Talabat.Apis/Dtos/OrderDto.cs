using System.ComponentModel.DataAnnotations;
using Talabat.Core.Models.Order_Aggregate;

namespace Talabat.APIs.Dtos
{
    public class OrderDto
    {
        [Required]
        public string BuyerEmail { get; set; } = null!;
        [Required]
        public string BasketId { get; set; } = null!;
        [Required]
        public Address ShippingAddress { get; set; } = null!;
        [Required]
        public int DeliveryMethodId { get; set; }
    }
}
