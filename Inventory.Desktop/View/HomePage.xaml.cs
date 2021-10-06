using System.Windows.Controls;
using Inventory.Desktop.ViewModel;

namespace Inventory.Desktop.View
{
    /// <summary>
    ///     Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : UserControl
    {

        public HomePage(HomeViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}