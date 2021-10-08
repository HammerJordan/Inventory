using Inventory.Remote.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using Infrastructure;
using Inventory.Application.Core;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Inventory.Remote
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("settings.json")
                .Build();

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();

            services.AddApplicationCore();
            services.AddInfrastructure(config);

            services.AddControllers();


            services.AddHttpClient();
        }

        private static void SetupLogger(IServiceCollection services, IConfiguration configuration)
        {
            string logPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            logPath = Path.Join(logPath, configuration["LoggingLocation"]);
            logPath = Path.Join(logPath, "Remote.log");
            var logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Logger = logger;


            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                Log.Fatal("Fatal Unhandled exception  {args}", args.ExceptionObject.ToString());
            };

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });


        }
    }
}
