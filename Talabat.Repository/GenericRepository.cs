using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseModel
    {
        private readonly StoreContext _dbContext;


        public GenericRepository(StoreContext dbContext) // Ask CLR FOR Object From DbContext
        {
            _dbContext = dbContext;
        }

        #region Without Spec DP

        public async Task<IReadOnlyList<T>> GetAllAsync()
             => await _dbContext.Set<T>().ToListAsync();

        public async Task<T?> GetAsync(int id)
            => await _dbContext.Set<T>().FindAsync(id); // FindAsync is optimized for primary key lookups 

        #endregion

        #region With Specifications

        public async Task<T?> GetWithSpecAsync(ISpecifications<T> specs)
         => await ApplySepcifications(specs).AsNoTracking().FirstOrDefaultAsync();
        
        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> specs)
            => await ApplySepcifications(specs).AsNoTracking().ToListAsync();

        private IQueryable<T> ApplySepcifications(ISpecifications<T> specs)
            => SpecificationsEvaluator<T>.GetQuery(_dbContext.Set<T>(), specs);

        #endregion

    }
}
