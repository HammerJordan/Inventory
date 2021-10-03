using System;
using System.IO;
using Application.Common.Interfaces;
using Application.Models.Product.Queries;
using Application.Models.Record.Queries;
using Infrastructure.Database;
using Infrastructure.Database.Queries;
using Infrastructure.FileAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            AddDbConfig(services,configuration);

            services
                .AddTransient<IExportCsvFile, ExportRecordListToCsv>()
                .AddTransient<IApplicationDbAccess, DbAccess>()
                .AddTransient<IProductSearchQuery, ProductSearchQuery>()
                .AddTransient<IRecordListItemQuery, RecordListItemQuery>()
                .AddTransient<IRecordModelQuery, RecordModelQuery>();

            return services;
        }
        
        private static void AddDbConfig(IServiceCollection serviceCollection,IConfiguration config)
        {
            string pathToDb = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            pathToDb = Path.Join(pathToDb, config["DbLocation"]);
            var dbConnection = new DbConnection(pathToDb);
            serviceCollection.AddSingleton(dbConnection);
        }
    }
}