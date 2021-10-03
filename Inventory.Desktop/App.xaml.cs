using System;
using System.Windows;
using Infrastructure;
using Inventory.Application;
using Inventory.DataAccess;
using Inventory.DataAccess.Queries;
using Inventory.Desktop.PopupWindows;
using Inventory.Desktop.Services;
using Inventory.Desktop.View;
using Inventory.Desktop.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebScraping;

// ReSharper disable PossibleNullReferenceException

namespace Inventory.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
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
            base.OnStartup(e);
            ServiceCollection.GetService<MainWindow>().Show();
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

            services.AddApplication();
            services.AddInfrastructure(Configuration);


            ServiceCollection = services.BuildServiceProvider();
        }
    }
}