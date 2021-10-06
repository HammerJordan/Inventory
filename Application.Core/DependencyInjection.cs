using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Application.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationCore(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            

            return services;
        }
    }
}