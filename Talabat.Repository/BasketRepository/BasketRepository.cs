using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Models.Basket;
using Talabat.Core.Repositories.Contract;

namespace Talabat.Repository.BasketRepository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;

        // You need Object from Database Context or Redis Client to implement these methods.
        public BasketRepository(IConnectionMultiplexer redis) // Open Connection With Redis
        {
            _database = redis.GetDatabase(); // Get Redis Database
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
            =>await _database.KeyDeleteAsync(basketId);

        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
            var basket = await _database.StringGetAsync(basketId);
            // Deserialize the basket from JSON to CustomerBasket object
            return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(basket!);
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
        {
            var basketJson = JsonSerializer.Serialize(basket);
            var CreatedOrUpdated = await _database.StringSetAsync(basket.Id, basketJson, TimeSpan.FromDays(30)); // Set the basket with a 30-day expiration
            if (!CreatedOrUpdated)
                return null; // If the operation failed, return null
            // If successful, retrieve the updated basket
            return await GetBasketAsync(basket.Id);
        }
    }
}
