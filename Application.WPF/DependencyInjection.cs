using Application.WPF.WebScraping;
using Application.WPF.WebScraping.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.WPF
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationWpf(this IServiceCollection services)
        {
            services
                .AddTransient<IProductUpdateRunner, ProductUpdateRunner>()
                .AddTransient<WebPageLoader>()
                .AddTransient<ProductScraper>();
            return services;

        }
    }
}