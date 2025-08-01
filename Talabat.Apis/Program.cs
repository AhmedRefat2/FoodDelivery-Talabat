using Microsoft.EntityFrameworkCore;
using Talabat.Apis.Helpers;
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
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            webApplicationBuilder.Services.AddEndpointsApiExplorer();
            webApplicationBuilder.Services.AddSwaggerGen();

            // 1. Configure the Database Context
            webApplicationBuilder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(webApplicationBuilder.Configuration
                       .GetConnectionString("DefaultConnection"));
            });

            // 2. Configure Repositories
            webApplicationBuilder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // 3. Configure Auto Mapper
            //webApplicationBuilder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));
            webApplicationBuilder.Services.AddAutoMapper(typeof(MappingProfiles)); 

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

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS

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
