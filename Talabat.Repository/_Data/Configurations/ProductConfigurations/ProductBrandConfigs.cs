﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models.Product;

namespace Talabat.Repository._Data.Configurations.ProductConfigurations
{
    internal class ProductBrandConfigs : IEntityTypeConfiguration<ProductBrand>
    {
        public void Configure(EntityTypeBuilder<ProductBrand> brand)
        {
            brand.Property(b => b.Name).IsRequired();
        }
    }
}
