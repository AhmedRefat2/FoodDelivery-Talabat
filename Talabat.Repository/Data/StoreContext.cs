using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Repository.Data.Configurations;

namespace Talabat.Repository.Data
{
    public class StoreContext : DbContext
    {

        // Dependaancy Injection Way : Chain On Parmaterized Constuctor(Options)
        // You need to Configure these Options 
        public StoreContext(DbContextOptions<StoreContext> options) 
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Fluent API Configuration 
            //modelBuilder.ApplyConfiguration(new ProductConfig());
            //modelBuilder.ApplyConfiguration(new ProductBrandConfigs());
            //modelBuilder.ApplyConfiguration(new ProductCategoryConfigs());

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); // Based On Reflection : Small Code And At run time Will Have Converted Into Base Code like Data Annotations     
              
        }


        // DbSet<T> : Represents a collection of entities of a specific type

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }


    }
}
