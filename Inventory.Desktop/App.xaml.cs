using System;
using System.IO;
using System.Reflection;
using System.Windows;
using Application.WPF;
using Infrastructure;
using Inventory.Application.Core;
using Inventory.Desktop.PopupWindows;
using Inventory.Desktop.Services;
using Inventory.Desktop.View;
using Inventory.Desktop.ViewModel;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

// ReSharper disable PossibleNullReferenceException

namespace Inventory.Desktop
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public IConfiguration Configuration { get; private set; }
        public IServiceProvider ServiceCollection { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("settings.json")
                .Build();

            Setup();

            SetupExceptionLogging();

            base.OnStartup(e);
            ServiceCollection.GetService<MainWindow>().Show();
            
            
        }

        private static void SetupExceptionLogging()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                Log.Fatal("Fatal Unhandled exception  {args}", args.ExceptionObject.ToString());
            };
        }

        private void Setup()
        {
            IServiceCollection services = new ServiceCollection();


            services
                .AddSingleton(Configuration)
                .AddSingleton<MainWindow>()
                .AddSingleton<MainWindowViewModel>();

            services
                .AddTransient<HomePage>()
                .AddTransient<HomeViewModel>()
                .AddTransient<CatalogPage>()
                .AddTransient<CatalogViewModel>()
                .AddTransient<SettingsPage>()
                .AddTransient<SettingsViewModel>()
                .AddTransient<SelectRecordWindow>()
                .AddTransient<SelectRecordWindowViewModel>()
                .AddTransient<ViewResolveService>();

            services.AddApplicationWpf(Configuration);
            services.AddApplicationCore();
            services.AddInfrastructure(Configuration);
            services.AddMediatR(Assembly.GetExecutingAssembly());

            SetupLogger(services, Configuration);

            ServiceCollection = services.BuildServiceProvider();
        }

        private static void SetupLogger(IServiceCollection services, IConfiguration configuration)
        {
            string logPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            logPath = Path.Join(logPath, configuration["LoggingLocation"]);
            logPath = Path.Join(logPath, ".log");
            var logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Logger = logger;
        }
    }
}