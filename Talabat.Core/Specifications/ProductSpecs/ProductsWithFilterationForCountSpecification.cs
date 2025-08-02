using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabat.Core.Specifications.ProductSpecs
{
    public class ProductsWithFilterationForCountSpecification : BaseSpecifications<Product>
    {
        public ProductsWithFilterationForCountSpecification(ProductSpecParams specParams)
            : base(P =>
                specParams != null &&
                (string.IsNullOrEmpty(specParams.Search) || P.Name.ToLower().Contains(specParams.Search)) &&
                (!specParams.BrandId.HasValue || P.BrandId == specParams.BrandId.Value) &&
                (!specParams.CategoryId.HasValue || P.CategoryId == specParams.CategoryId.Value)
            )
        { }
    }
}
