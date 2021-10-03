using System;
using System.IO;
using System.Windows;
using Inventory.Domain;
using Inventory.Domain.IoC;
using Inventory.Domain.Services;
using Inventory.DataAccess;
using Inventory.DataAccess.Queries;
using Inventory.Desktop.PopupWindows;
using Inventory.Desktop.Services;
using Inventory.Desktop.View;
using Inventory.Desktop.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PubSub;
using WebScraping;

// ReSharper disable PossibleNullReferenceException

namespace Inventory.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            SetupIoC();
            base.OnStartup(e);
            IoC.Get<MainWindow>().Show();
        }

        private void SetupIoC()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("settings.json")
                .Build();

            var serviceCollection = new ServiceCollection();
            serviceCollection
                .AddSingleton(config)
                .AddSingleton<MainWindow>()
                .AddSingleton<MainWindowViewModel>();

            // views and vm
            serviceCollection
                .AddTransient<HomePage>()
                .AddTransient<HomeViewModel>()
                .AddTransient<CatalogPage>()
                .AddTransient<CatalogViewModel>()
                .AddTransient<SettingsPage>()
                .AddTransient<SettingsViewModel>()
                .AddTransient<SelectRecordWindow>()
                .AddTransient<SelectRecordWindowViewModel>()
                .AddTransient<ViewResolveService>()
                .AddTransient<IRecordQuery, RecordQuery>()
                .AddTransient<IExportRecord, ExportRecord>()
                .AddTransient<IRecordItemsQuery, RecordItemsQuery>()
                .AddTransient<WebPageLoader>()
                .AddTransient<ProductScraper>()
                .AddTransient<ProductUpdateRunner>()
                .AddTransient<ISqlLiteDataAccess, SqlLiteDataAccess>()
                .AddTransient<ProductSearchEngine>();

            AddDbConfig(config, serviceCollection);

            var services = serviceCollection.BuildServiceProvider();

            IoC.IoCInitialize(services);
        }

        private static void AddDbConfig(IConfiguration config, ServiceCollection serviceCollection)
        {
            string pathToDb = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            pathToDb = Path.Join(pathToDb, config["DbLocation"]);
            var dbConnection = new DbConnection(pathToDb);

            serviceCollection.AddSingleton(dbConnection);
        }
    }
}