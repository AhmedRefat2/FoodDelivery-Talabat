using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositories.Contract
{
    public interface IGenericRepository<T> where T : BaseModel
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T?> GetAsync(int id);

        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> specs);
        Task<T?> GetWithSpecAsync(ISpecifications<T> specs);
        Task<int> GetCountAsync(ISpecifications<T> specs);

        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);

    }
}
