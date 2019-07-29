namespace ProductService {
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using System;
    using AutoMapper;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.HttpsPolicy;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using ProductService.Configuration;
    using ProductService.Domain;
    using ProductService.Events;
    using ProductService.Formatters;
    using ProductService.Persistence;
    using ProductService.Services.Abstract;
    using ProductService.Services;
    using Swashbuckle.AspNetCore.Swagger;

    public class Startup {

        private readonly ILogger _logger;
        public IConfiguration Configuration { get; }

        public Startup (IConfiguration configuration, ILogger<Startup> logger) {
            Configuration = configuration ??
                throw new ArgumentNullException (nameof (Configuration));
            _logger = logger ??
                throw new ArgumentNullException (nameof (_logger));
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {

            //Set Product DB Connection
            services.AddDbContext<ProductDbContext>
                (options => options.UseSqlite (Configuration.GetConnectionString ("ProductDBConnection")));

            // Automapper
            services.AddAutoMapper (typeof (Startup));

            //Add Repositories
            services.AddTransient<IProductRepository, ProductRepository> ();

            //Service Bus Settings
            services.Configure<ServiceBus> (Configuration.GetSection (nameof (ServiceBus)));

            //Add MediatR
            services.AddMediatR (typeof (Startup).GetTypeInfo ().Assembly);

            //Add Service Bus
            services.AddTransient<IBusService, BusService> ();

            //Configure Swagger
            services.AddSwaggerGen (options => {
                options.SwaggerDoc ("v1", new Info { Title = "MultiShop Product APIs", Description = "Product APIs", Version = "v1" });
            });

            services.AddMvc ().SetCompatibilityVersion (CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            } else {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts ();
            }

            // Create Database if not created.
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory> ().CreateScope ()) {
                var context = serviceScope.ServiceProvider.GetRequiredService<ProductDbContext> ();
                context.Database.EnsureCreated ();
            }

            //Swagger
            app.UseSwagger ();
            app.UseSwaggerUI (s => {
                s.SwaggerEndpoint ("/swagger/v1/swagger.json", "MultiShop Product API");
            });

            app.UseHttpsRedirection ();
            app.UseMvc ();
        }
    }
}