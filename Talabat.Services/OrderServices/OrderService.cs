
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models.Order_Aggregate;
using Talabat.Core.Models.Product;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;

namespace Talabat.Services.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepo;
        private readonly IGenericRepository<Order> _orderRepo;

        public OrderService(IBasketRepository basketRepo,
            IGenericRepository<Product> productRepo,
            IGenericRepository<DeliveryMethod> deliveryMethodRepo,
            IGenericRepository<Order> orderRepo)
        {
            _basketRepo = basketRepo;
            _productRepo = productRepo;
            _deliveryMethodRepo = deliveryMethodRepo;
            _orderRepo = orderRepo;
        }
        public async Task<Order> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            // 1.Get Basket From Baskets Repo

            var basket = await _basketRepo.GetBasketAsync(basketId);

            // 2. Get Selected Items at Basket From Products Repo

            var orderItems = new List<OrderItem>();

            if (basket?.Items?.Count > 0)
            {
                // Loop For Each Item At Basket
                foreach (var item in basket.Items)
                {
                    // Get Product From Products Repo

                    var product = await _productRepo.GetAsync(item.Id);

                    // Create OrderItem For Each Item

                    if(product != null)
                    {
                        var productItemOrdered = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);

                        var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);

                        orderItems.Add(orderItem);
                    }
                }
            }

            // 3. Calculate SubTotal
            
            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);

            // 4. Get Delivery Method From DeliveryMethods Repo

            //var deliveryMethod = await _deliveryMethodRepo.GetAsync(deliveryMethodId);

            // 5. Create Order

            var order = new Order(buyerEmail, shippingAddress, deliveryMethodId, orderItems, subTotal);   


            _orderRepo.Add(order); // Added Locally 

            // 6. Save To Database [TODO]

            return order;



        }
        public Task<Order> GetOrderByIdForUserAsync(string buyerEmail, string orderId)
        {
            throw new NotImplementedException();
        }
        public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            throw new NotImplementedException();
        }


    }
}
