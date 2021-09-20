using System.Windows.Controls;
using Inventory.Desktop.ViewModel;

namespace Inventory.Desktop.View
{
    /// <summary>
    /// Interaction logic for CatalogPage.xaml
    /// </summary>
    public partial class CatalogPage : UserControl
    {
        private readonly CatalogViewModel viewModel;
        public CatalogPage(CatalogViewModel viewModel)
        {
            this.viewModel = viewModel;
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
