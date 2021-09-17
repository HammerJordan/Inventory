using System.Collections.Generic;
using System.Windows.Controls;
using InventoryManagement.Core.IoC;
using InventoryManagement.Desktop.View;

namespace InventoryManagement.Desktop.Services
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
                "Record" => IoC.Get<RecordPage>(),
                "Catalog" => IoC.Get<CatalogPage>(),
                "Settings" => IoC.Get<SettingsPage>(),
                _ => null,
            };
        }

    }
}