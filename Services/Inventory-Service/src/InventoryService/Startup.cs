using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hangfire;
using Hangfire.LiteDB;
using InventoryService.Commands;
using InventoryService.Configuration;
using InventoryService.Domain;
using InventoryService.Events;
using InventoryService.Events.Handlers;
using InventoryService.Persistence;
using InventoryService.Services;
using InventoryService.Services.Abstract;
using InventoryService.Utilities;
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

namespace InventoryService {
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

           
            //Set Inventory DB Connection
            services.AddDbContext<InventoryDbContext> (options => options.UseSqlite (Configuration.GetConnectionString ("InventoryConnection")),ServiceLifetime.Singleton);

            //Add Repositories
            services.AddTransient<IInventoryStoreRepository, InventoryStoreRepository> ();

            //Service Bus Settings
            services.Configure<ServiceBus> (Configuration.GetSection (nameof (ServiceBus)));

            //Register EventServices
            services.AddSingleton<IEventService, ProductUpdatedEventService> ();
            services.AddSingleton<IEventService, ProductAddedEventService> ();
             services.AddSingleton<IEventService, ProductDeletedEventService> ();

            //Add Hangfire
            services.AddHangfire (t => t.UseLiteDbStorage (Configuration.GetConnectionString ("HangfireConnection")));
            
             // Automapper
            services.AddAutoMapper (typeof (Startup));

            //Add MediatR
            services.AddMediatR (typeof (Startup).GetTypeInfo ().Assembly);

            services.AddMvc ().SetCompatibilityVersion (CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            } else {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts ();
            }

            // Create Database if not created.
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory> ().CreateScope ()) {
                var context = serviceScope.ServiceProvider.GetRequiredService<InventoryDbContext> ();
                context.Database.EnsureCreated ();
            }

            // Hangfire configuration 
            GlobalConfiguration.Configuration.UseLiteDbStorage ();
            app.UseHangfireDashboard ();
            app.UseHangfireServer ();

            //Schedule recurring jobs
            List<IEventService> servicebuses = app.ApplicationServices.GetServices<IEventService> ().ToList ();
            foreach (IEventService service in servicebuses) {
                RecurringJob.AddOrUpdate (service.ToString (), () => service.Subscribe ().GetAwaiter ().GetResult (), Cron.Minutely, TimeZoneInfo.Utc, "inventoryservicequeue");
            }

            app.UseHttpsRedirection ();
            app.UseMvc ();
        }
    }
}