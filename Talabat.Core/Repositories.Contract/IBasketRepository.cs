using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabat.Core.Repositories.Contract
{
    public interface IBasketRepository
    {
        // get Basket
        Task<CustomerBasket?> GetBasketAsync(string basketId);
        // Create Or Update 
        Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket);
        // Delete Basket
        Task<bool> DeleteBasketAsync(string basketId);
    }
}
