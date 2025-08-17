using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models.Product;

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
        public ProductWithBrandAndCategorySpecifications(ProductSpecParams specParams)
            : base(P =>
                specParams != null&&
                (string.IsNullOrEmpty(specParams.Search) || P.Name.ToLower().Contains(specParams.Search.ToLower())) && 
                (!specParams.BrandId.HasValue || P.BrandId == specParams.BrandId.Value) &&
                (!specParams.CategoryId.HasValue || P.CategoryId == specParams.CategoryId.Value)
            )
        {
            AddIncludes();

            if (!string.IsNullOrEmpty(specParams?.Sort))
            {
                switch (specParams?.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDesc(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }

            else
                AddOrderBy(P => P.Name);

            ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
        }



        private void AddIncludes()
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Category);
        }
    }
}
