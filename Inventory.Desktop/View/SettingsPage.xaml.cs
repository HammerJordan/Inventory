using System.Windows.Controls;
using Inventory.Desktop.ViewModel;

namespace Inventory.Desktop.View
{
    /// <summary>
    ///     Interaction logic for SettingsPage.xaml
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