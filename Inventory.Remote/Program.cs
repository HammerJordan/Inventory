using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Remote
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var configuration = new ConfigurationBuilder()
            //    .AddCommandLine(args)
            //    .Build();


            //var hostUrl = configuration["hosturl"]; // add this line
            //if (string.IsNullOrEmpty(hostUrl)) // add this line
            //    hostUrl = "http://0.0.0.0:5001"; // add this line


            //var host = new WebHostBuilder()
            //    .UseKestrel()
            //    .UseUrls(hostUrl)   // // add this line
            //    .UseContentRoot(Directory.GetCurrentDirectory())
            //    .UseIISIntegration()
            //    .UseStartup<Startup>()
            //    .UseConfiguration(configuration)
                
            //    .Build();

            //host.Run();


            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
