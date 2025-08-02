﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabat.Core.Specifications
{
    public class BaseSpecifications<T> : ISpecifications<T> where T : BaseModel
    {
        public Expression<Func<T, bool>> Criteria { get; set; } = null!;
        public Expression<Func<T, object>> OrderBy { get; private set; } = null!;
        public Expression<Func<T, object>> OrderByDesc { get; private set; } = null!;
        public List<Expression<Func<T, object>>> Includes { get ; set ; } = new List<Expression<Func<T, object>>> ();

        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginationEnabled { get; set; } 

        // CTOR FOR GET ALL Without Crieteria
        public BaseSpecifications()
        {
            
        }
        // CTOR FOR GET Specific According to Condition
        public BaseSpecifications(Expression<Func<T, bool>> criteriaExpression)
        {
            Criteria = criteriaExpression;    
        }

        // Setters For Order By And Order By Descending
        public void AddOrderBy(Expression<Func<T, object>> orderByExpression) 
        {
            OrderBy = orderByExpression;
        }

        public void AddOrderByDesc(Expression<Func<T, object>> orderByDescExpression)
        {
            OrderByDesc = orderByDescExpression;
        }

        // Apply Pagination 
        public void ApplyPagination(int skip, int take)
        {
            IsPaginationEnabled = true;
            Skip = skip;
            Take = take;
        }
    }
}
