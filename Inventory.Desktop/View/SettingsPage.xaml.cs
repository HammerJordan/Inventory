using System.Windows.Controls;
using InventoryManagement.Desktop.ViewModel;

namespace InventoryManagement.Desktop.View
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : UserControl
    {
        public SettingsPage(SettingsViewModel settingsVm)
        {
            DataContext = settingsVm;
            InitializeComponent();
        }
    }
}
