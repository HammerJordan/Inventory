using System.Collections.Generic;
using System.Windows.Controls;
using Inventory.Core.IoC;
using Inventory.Desktop.View;

namespace Inventory.Desktop.Services
{
    public class ViewResolveService
    {
        private Dictionary<string, UserControl> cachedViews = new Dictionary<string, UserControl>();



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
                "Home" => IoC.Get<HomePage>(),
                "Catalog" => IoC.Get<CatalogPage>(),
                "Settings" => IoC.Get<SettingsPage>(),
                _ => null,
            };
        }

    }
}