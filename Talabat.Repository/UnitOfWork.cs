using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Models;
using Talabat.Core.Models.Order_Aggregate;
using Talabat.Core.Models.Product;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository.Data;
using Talabat.Repository.GenericRepository;

namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
    #region OLD Unit Of Work : has Problem of Open For Extenstion Closed For Modifications and UnNecessary Repositories
        //public IGenericRepository<Product> ProductsRepo { get; set; }
        //public IGenericRepository<ProductCategory> CategorysRepo { get; set; }
        //public IGenericRepository<ProductBrand> BrandsRepo { get; set; }
        //public IGenericRepository<DeliveryMethod> DeliveryMethodsRepo { get; set; }
        //public IGenericRepository<OrderItem> OrderItemsRepo { get; set; }
        //public IGenericRepository<Order> OrdersRepo { get; set; } 
        #endregion

        private readonly StoreContext _dbContext;

        // private Dictionary<string, GenericRepository<BaseModel>> _repositories; 
        private Hashtable _repositories;
        public UnitOfWork(StoreContext dbContext) // ASK CLR FOR CREATING OBJECT FROM DB CONTEXT Implicitly
        {
            _dbContext = dbContext;
            _repositories = new Hashtable();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseModel
        {
            var Key = typeof(TEntity).Name; // Order 
            if (!_repositories.ContainsKey(Key))
            {
                var repository = new GenericRepository<TEntity>(_dbContext);
                _repositories.Add(Key, repository);
            }
            return _repositories[Key] as IGenericRepository<TEntity>;
                
        }

        public async Task<int> CompleteAsync()
            => await _dbContext.SaveChangesAsync();

        public async ValueTask DisposeAsync()
            => await _dbContext.DisposeAsync();
 
    }
}
