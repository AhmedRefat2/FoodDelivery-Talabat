using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models.Product;

namespace Talabat.Repository.Data.Configurations
{
    internal class ProductCategoryConfigs : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> category)
        {
            category.Property(c => c.Name).IsRequired();
        }
    }
  
}
