using System;
using System.IO;
using System.Reflection;
using Application.Core.Common.Interfaces;
using Application.Core.Models.Product.Queries;
using Application.Core.Models.Record.Queries;
using Application.Core.Models.RecordProductList.Queries;
using Infrastructure.Database;
using Infrastructure.Database.Queries;
using Infrastructure.FileAccess;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;


namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            AddDbConfig(services,configuration);
            
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services
                .AddTransient<IExportCsvFile, ExportRecordListToCsv>()
                .AddTransient<IApplicationDbAccess, DbAccess>()
                .AddTransient<IProductSearchQuery, ProductSearchQuery>()
                .AddTransient<IRecordListItemQuery, RecordListItemQuery>()
                .AddTransient<IRecordModelQuery, RecordModelQuery>()
                .AddTransient<IProductModelQuery, ProductModelQuery>();
            
           

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