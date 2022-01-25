using CarsPool.Api.Abstractions;
using CarsPool.Api.Filters;
using CarsPool.Api.Services;
using CarsPool.Dal.Abstractions;
using CarsPool.Dal.Contexts;
using CarsPool.Dal.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace CarsPool.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetAssembly(typeof(Startup)), Assembly.GetAssembly(typeof(CarsPoolDbContext)));

            services.AddDbContext<CarsPoolDbContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("CarsPoolDb"));
            })
            .AddTransient<ICarRepository, CarRepository>()
            .AddTransient<ICarService, CarService>()
            .AddTransient<IDriverRepository, DriverRepository>()
            .AddTransient<IDriverService, DriverService>()
            ;

            services.AddControllers(opt =>
            {
                opt.Filters.Add(new ExceptionsHandlingFilter());
            })
            .AddNewtonsoftJson();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CarsPool.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CarsPool.Api v1"));
            }

            app.UseRouting();

            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
