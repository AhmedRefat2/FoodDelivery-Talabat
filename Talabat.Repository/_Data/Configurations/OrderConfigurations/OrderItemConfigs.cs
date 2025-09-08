using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models.Order_Aggregate;

namespace Talabat.Repository._Data.Configurations.OrderConfigurations
{
    internal class OrderItemConfigs : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.OwnsOne(OrderItem => OrderItem.Product, Product => Product.WithOwner());

            builder.Property(orderItem => orderItem.Price)
                .HasColumnType("decimal(12,2)");
        }
    }
}
