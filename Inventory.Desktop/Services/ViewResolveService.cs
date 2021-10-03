using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Inventory.Desktop.View;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Desktop.Services
{
    public class ViewResolveService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<string, UserControl> cachedViews = new();

        public ViewResolveService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public UserControl Resolve(string name)
        {
            if (cachedViews.ContainsKey(name))
                return cachedViews[name];

            var view = GetNewView(name);
            cachedViews.Add(name,view);

            return view;
        }

        private UserControl GetNewView(string name)
        {
            return name switch
            {
                "Home" => _serviceProvider.GetService<HomePage>(),
                "Catalog" => _serviceProvider.GetService<CatalogPage>(),
                "Settings" => _serviceProvider.GetService<SettingsPage>(),
                _ => null,
            };
        }

    }
}