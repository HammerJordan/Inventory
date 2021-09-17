using System.Windows;
using InventoryManagement.Desktop.Controls;
using InventoryManagement.Desktop.Services;
using InventoryManagement.Desktop.ViewModel;

namespace InventoryManagement.Desktop
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel viewModel;
        private readonly ViewResolveService viewResolve;

        public MainWindow(MainWindowViewModel viewModel, ViewResolveService viewResolve)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            this.viewResolve = viewResolve;
            DataContext = viewModel;

            foreach (UIElement child in SideBarNav.Children)
            {
                if (child is SideBarItem sideBarItem)
                {
                    sideBarItem.SideBarClickEvent += OnSideBarItemClickedEvent;

                    if (sideBarItem.SideBarSelected)
                        ActiveContent.Content = viewResolve.Resolve(sideBarItem.SideBarName);

                }
            }
        }

        private void OnSideBarItemClickedEvent(SideBarItem clickedItem)
        {
            foreach (UIElement child in SideBarNav.Children)
            {
                if (child is SideBarItem sideBarItem)
                {
                    if (sideBarItem == clickedItem)
                    {
                        sideBarItem.SideBarSelected = true;
                        ActiveContent.Content = viewResolve.Resolve(sideBarItem.SideBarName);
                    }
                    else
                    {
                        sideBarItem.SideBarSelected = false;
                    }

                }
            }
        }

    }
}