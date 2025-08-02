using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabat.Core.Specifications
{
    public interface ISpecifications<T> where T : BaseModel
    {
        // Signuture For Props For Specs
        // 1. Where
        public Expression<Func<T, bool>> Criteria { get; set; }
        // 2. Includes 
        public List<Expression<Func<T, object>>> Includes { get; set; }
        // 3. Order By 
        public Expression<Func<T, object>> OrderBy { get; }
        // 4. Order By Descending
        public Expression<Func<T, object>> OrderByDesc { get; }

        // 5. Skip 
        public int Skip { get; set; }
        // 6. Take
        public int Take { get; set; }

        // 7. Is Pagination Enabled
        public bool IsPaginationEnabled { get; set; }

        // 8. Search 
        public string? Search { get; set; }
    }
}
