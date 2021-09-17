using System.Windows;
using InventoryManagement.Core.IoC;
using InventoryManagement.Desktop.Services;
using InventoryManagement.Desktop.View;
using InventoryManagement.Desktop.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable PossibleNullReferenceException

namespace InventoryManagement.Desktop
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



            //var x = config.GetSection("IgnoreProducts");
            //var c = x.GetChildren().Select(x => x.Value).ToArray();

            //TestDB();


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
                .AddTransient<RecordPage>()
                .AddTransient<RecordViewModel>()
                .AddTransient<CatalogPage>()
                .AddTransient<CatalogViewModel>()
                .AddTransient<SettingsPage>()
                .AddTransient<SettingsViewModel>(); ;


            serviceCollection
                .AddTransient<ViewResolveService>();
                // .AddTransient<InvoiceDBHelper>()
                // .AddTransient<WebPageLoader>()
                // .AddTransient<DatabaseUpdate>()
                // .AddTransient<ProductScraper>()
                // .AddTransient<ProductUpdateRunner>()
                //
                // .AddTransient<ISqlLiteDataAccess, SqlLiteDataAccess>()
                // .AddTransient<ProductSearchEngine>();


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
