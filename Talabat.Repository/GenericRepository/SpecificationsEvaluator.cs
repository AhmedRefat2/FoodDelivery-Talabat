using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Specifications;

namespace Talabat.Repository.GenericRepository
{
    internal static class SpecificationsEvaluator<TEntity> where TEntity : BaseModel
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecifications<TEntity> specs)
        {
            var query = inputQuery; // _dbContext.Set<TEntity>

            // 1. Criteria
            if(specs.Criteria is not null) // E => E.Id == 1
                query = query.Where(specs.Criteria);
            // query = _dbContext.Set<TEntity>().Where(criteria);

            // 2. Includes 
            if (specs.Includes is not null && specs.Includes.Count() > 0)
                query = specs.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));

            // 3. Order By
            if (specs.OrderBy is not null)
                query = query.OrderBy(specs.OrderBy);

            // 4. Order By Descending
            if (specs.OrderByDesc is not null)
                query = query.OrderByDescending(specs.OrderByDesc);

            // 5. Pagination
            if (specs.IsPaginationEnabled)
                query = query.Skip(specs.Skip).Take(specs.Take);


            return query;
        }

    }
}
