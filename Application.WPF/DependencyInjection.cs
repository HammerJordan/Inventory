using System.Reflection;
using Application.WPF.WebScraping;
using Application.WPF.WebScraping.Common;
using Application.WPF.WebScraping.ProductUpdates;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.WPF
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationWpf(this IServiceCollection services, IConfiguration configuration)
        {
            var productUpdateRule = new ProductUpdateRule(configuration);
            services.AddSingleton(productUpdateRule);
            
            services.AddMediatR(Assembly.GetExecutingAssembly());
            
            services
                .AddTransient<IProductUpdateRunner, ProductUpdateRunner>()
                .AddTransient<WebPageLoader>()
                .AddTransient<ProductScraper>();
            return services;

        }
    }
}