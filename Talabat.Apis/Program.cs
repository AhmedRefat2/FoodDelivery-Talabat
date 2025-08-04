using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.Apis.Extensions;
using Talabat.Apis.Helpers;
using Talabat.Apis.Middlewares;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository;
using Talabat.Repository.Data;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var webApplicationBuilder = WebApplication.CreateBuilder(args);

            #region Configure Services 

            webApplicationBuilder.Services.AddControllers();
            webApplicationBuilder.Services.AddSwaggerServices(); // Extension Method 

            // 1. Configure the Database Context
            webApplicationBuilder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(webApplicationBuilder.Configuration
                       .GetConnectionString("DefaultConnection"));
            });


            // Add Services Of Redis Database 
            webApplicationBuilder.Services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
            {
                var connection = webApplicationBuilder.Configuration.GetConnectionString("RedisConnection");
                return ConnectionMultiplexer.Connect(connection);
            });

            webApplicationBuilder.Services.AddApplicationServices(); // AutoMapper, Generic Repo, Validation Error

            #endregion


            var app = webApplicationBuilder.Build();

            #region Update Database  & Data Seeding 
            
            // 1. Create Scope & Dispose It 
            using var scope = app.Services.CreateScope();

            // 2. Get the Service Provider from the Scope
            var services = scope.ServiceProvider;

            // 3. Get the DbContext from the Service Provider
            var _dbContext = services.GetRequiredService<StoreContext>();

            // 4. Get Looger 

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            // 4. Apply Migrations and Update the Database
            try
            {
                await _dbContext.Database.MigrateAsync(); // Apply Migrations  

                // Seed Data
                await StoreContextSeed.SeedAsync(_dbContext); // Seed Data from JSON files
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error occurred during apply the migration");
            }



            // Data Seed 

            #endregion


            #region Configure Middlewares [Http Request Pipline]

            // ExceptionMiddleware
            app.UseMiddleware<ExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddleware();
            }

            app.UseHttpsRedirection(); 
            app.UseStatusCodePagesWithReExecute("/errors/{0}"); 

            app.UseStaticFiles();

            app.UseAuthorization(); // Authorize the requests


            app.MapControllers(); // Map the controllers to the HTTP request pipeline [API] .NET 6+ feature

            #region Routing MVC 
            //app.UseRouting();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Home}/{action=Index}/{id?}"
            //    );
            //}); 
            #endregion


            #endregion

            app.Run();
        }
    }
}
