using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Models.Order_Aggregate;
using Talabat.Core.Models.Product;
using Talabat.Core.Repositories.Contract;

namespace Talabat.Core
{
    public interface IUnitOfWork : IAsyncDisposable 
    {
        #region OLD Unit Of Work : has Problem of Open For Extenstion Closed For Modifications and UnNecessary Repositories

        // Property Signuture For Each and Every Repository 
        //public IGenericRepository<Product> ProductsRepo{ get; set; }
        //public IGenericRepository<ProductCategory> CategorysRepo{ get; set; }
        //public IGenericRepository<ProductBrand> BrandsRepo{ get; set; }
        //public IGenericRepository<DeliveryMethod> DeliveryMethodsRepo{ get; set; }
        //public IGenericRepository<OrderItem> OrderItemsRepo{ get; set; }
        //public IGenericRepository<Order> OrdersRepo{ get; set; } 
        #endregion
        // Method That Create Needed Repository Object
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseModel;

        Task<int> CompleteAsync();
    }
}
