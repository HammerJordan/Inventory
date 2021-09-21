using System.Windows;
using System.Windows.Controls.Primitives;
using Inventory.Desktop.Services;
using Inventory.Desktop.ViewModel;
using Inventory.Desktop.Controls;
using Inventory.Desktop.View;
using SideBarItem = Inventory.Desktop.Controls.SideBarItem;

namespace Inventory.Desktop
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
                if (child is Controls.SideBarItem sideBarItem)
                {
                    sideBarItem.SideBarClickEvent += OnSideBarItemClickedEvent;

                    if (sideBarItem.SideBarSelected)
                        ActiveContent.Content = viewResolve.Resolve(sideBarItem.SideBarName);

                }
            }


        }

        private void OnSideBarItemClickedEvent(Controls.SideBarItem clickedItem)
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