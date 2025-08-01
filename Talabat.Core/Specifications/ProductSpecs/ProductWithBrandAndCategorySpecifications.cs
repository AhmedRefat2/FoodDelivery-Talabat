using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabat.Core.Specifications.ProductSpecs
{
    public class ProductWithBrandAndCategorySpecifications : BaseSpecifications<Product>
    {
        // GET By ID
        public ProductWithBrandAndCategorySpecifications(int id) : base(p => p.Id == id)
        {
            AddIncludes();
        }

        // GET ALL
        public ProductWithBrandAndCategorySpecifications() : base()
        {
            AddIncludes();
        }

        private void AddIncludes()
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Category);
        }
    }
}
