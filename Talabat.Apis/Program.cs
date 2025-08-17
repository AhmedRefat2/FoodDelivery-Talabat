using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Text;
using Talabat.Apis.Extensions;
using Talabat.Apis.Helpers;
using Talabat.Apis.Middlewares;
using Talabat.Core.Models.Identity;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Repository;
using Talabat.Repository._Identity;
using Talabat.Repository.Data;
using Talabat.Services.AuthService;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var webApplicationBuilder = WebApplication.CreateBuilder(args);

            #region Configure Services 

            webApplicationBuilder.Services.AddControllers().AddNewtonsoftJson(Options => { 
                
                Options.SerializerSettings.ReferenceLoopHandling= ReferenceLoopHandling.Ignore; // Ignore Reference Loop Handling
            });

            webApplicationBuilder.Services.AddSwaggerServices();

            webApplicationBuilder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(webApplicationBuilder.Configuration
                       .GetConnectionString("DefaultConnection"));
            });

            webApplicationBuilder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
            {
                options.UseSqlServer(webApplicationBuilder.Configuration
                       .GetConnectionString("IdentityConnection"));
            });

            webApplicationBuilder.Services.AddApplicationServices(); 

            webApplicationBuilder.Services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
            {
                var connection = webApplicationBuilder.Configuration.GetConnectionString("RedisConnection");
                return ConnectionMultiplexer.Connect(connection);
            });

            webApplicationBuilder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationIdentityDbContext>();

            webApplicationBuilder.Services.AddAuthServices(webApplicationBuilder.Configuration); // Extension Method For Security Services 

            #endregion


            var app = webApplicationBuilder.Build();
            
            #region Update Database  & Data Seeding 
            
            // 1. Create Scope & Dispose It 
            using var scope = app.Services.CreateScope();

            // 2. Get the Service Provider from the Scope
            var services = scope.ServiceProvider;

            // 3. Get the DbContext from the Service Provider
            var _dbContext = services.GetRequiredService<StoreContext>();

            // 3. Get the Identity DbContext from the Service Provider
            var _identityDbContext = services.GetRequiredService<ApplicationIdentityDbContext>();

            // 4. Get Looger 

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            // 4. Apply Migrations and Update the Database
            try
            {
                await _dbContext.Database.MigrateAsync(); // Apply Migrations  
                await _identityDbContext.Database.MigrateAsync(); // Apply Migrations for Identity Database

                // Seed Data For Products, Brands, Categories
                await StoreContextSeed.SeedAsync(_dbContext); // Seed Data from JSON files

                // Seed Data For Identity Users
                var userMangerService = services.GetRequiredService<UserManager<ApplicationUser>>();
                await ApplicationIdentityContextSeed.SeedUsersAsync(userMangerService); 
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

            app.UseAuthentication();

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
