using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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
using ShoppingCartService.Configuration;
using ShoppingCartService.Domain;
using ShoppingCartService.Persistence;
using ShoppingCartService.Services;
using ShoppingCartService.Services.Abstract;
using Swashbuckle.AspNetCore.Swagger;

namespace ShoppingCartService {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            //Set ShoppingCart DB Connection
            services.AddDbContext<ShoppingCartDbContext> (options => options.UseSqlite (Configuration.GetConnectionString ("ShoppingCartDBConnection")), ServiceLifetime.Singleton);

            //Service Bus Settings
            services.Configure<ServiceBus> (Configuration.GetSection (nameof (ServiceBus)));

            //HttpUrls Settings
            services.Configure<HttpUrls> (Configuration.GetSection (nameof (HttpUrls)));
            services.Configure<ProductServiceSettings> (Configuration.GetSection ($"{nameof (HttpUrls)}:{nameof (ProductServiceSettings)}"));

            //Add Repositories
            services.AddTransient<IShoppingCartRepository, ShoppingCartRepository> ();

            //Add HTTP Services
            services.AddHttpClient<IProductService, ProductService> ();

            // Automapper
            services.AddAutoMapper (typeof (Startup));

            //Add MediatR
            services.AddMediatR (typeof (Startup).GetTypeInfo ().Assembly);

            //Configure Swagger
            services.AddSwaggerGen (options => {
                options.SwaggerDoc ("v1", new Info { Title = "MultiShop ShoppingCart APIs", Description = "ShoppingCart APIs", Version = "v1" });
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
                var context = serviceScope.ServiceProvider.GetRequiredService<ShoppingCartDbContext> ();
                context.Database.EnsureCreated ();
            }

            //Swagger
            app.UseSwagger ();
            app.UseSwaggerUI (s => {
                s.SwaggerEndpoint ("/swagger/v1/swagger.json", "MultiShop ShoppingCart API");
            });

            app.UseHttpsRedirection ();
            app.UseMvc ();
        }
    }
}