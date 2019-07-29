using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ShoppingCartService {
    public class Program {
        public static void Main (string[] args) {
            CreateWebHostBuilder (args).Build ().Run ();
        }

        public static IWebHostBuilder CreateWebHostBuilder (string[] args) =>
            WebHost.CreateDefaultBuilder (args)
            .UseSerilog ((ctx, config) => { config.ReadFrom.Configuration (ctx.Configuration); })
            .UseConfiguration (new ConfigurationBuilder ().SetBasePath (Directory.GetCurrentDirectory ())
                .AddJsonFile ("hosting.json", optional : true)
                .Build ())
            .UseStartup<Startup> ();
    }
}