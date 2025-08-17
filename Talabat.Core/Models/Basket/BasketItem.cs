namespace Talabat.Core.Models.Basket
{
    public class BasketItem : BaseModel
    {
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
    }
}