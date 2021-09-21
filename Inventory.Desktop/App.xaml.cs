using System;
using System.IO;
using System.Windows;
using Inventory.Core;
using Inventory.Core.IoC;
using Inventory.DataAccess;
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


            serviceCollection.AddSingleton(config);
            serviceCollection.AddSingleton<MainWindow>();
            serviceCollection.AddSingleton<MainWindowViewModel>();

            serviceCollection
                .AddTransient<HomePage>()
                .AddTransient<HomeViewModel>()
                .AddTransient<CatalogPage>()
                .AddTransient<CatalogViewModel>()
                .AddTransient<SettingsPage>()
                .AddTransient<SettingsViewModel>(); ;


            serviceCollection
                .AddTransient<ViewResolveService>()
                .AddTransient<InvoiceDBHelper>()
                .AddTransient<WebPageLoader>()
                .AddTransient<DatabaseUpdate>()
                .AddTransient<ProductScraper>()
                .AddTransient<ProductUpdateRunner>()
                .AddTransient<ISqlLiteDataAccess, SqlLiteDataAccess>()
                .AddTransient<ProductSearchEngine>();

            string pathToDb = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            pathToDb = Path.Join(pathToDb, config["DbLocation"]);
            var dbConnection = new DbConnection(pathToDb);

            serviceCollection.AddSingleton(dbConnection);


            var services = serviceCollection.BuildServiceProvider();

            IoC.IoCInitialize(services);


        }

        //private async Task TestDB()
        //{
        //    var connection = new DbConnection();
        //    using var webPageLoader = new WebPageLoader();
        //    var pageScraper = new ProductScraper(webPageLoader);

        //    var sw = new System.Diagnostics.Stopwatch();

        //    var ignoreCategories = ServiceCollection
        //        .GetService<IConfiguration>()
        //        .GetSection("IgnoreProducts")
        //        .GetChildren()
        //        .Select(x => x.Value)
        //        .ToArray();

        //    sw.Start();

        //    var results = await Task.Run(async () => await pageScraper.GetAllModelsFromCategoriesAsync(
        //         ServiceCollection.GetService<IConfiguration>()["EntryUrl"],
        //         ignoreCategories));

        //    await connection.InsertProductModels(results);







        //    //int count = 0;
        //    // foreach (var Model in results)
        //    // {
        //    //     count++;
        //    //     await connection.InsertProductModel(Model);
        //    // }


        //}
    }
}
