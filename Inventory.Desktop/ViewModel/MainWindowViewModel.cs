using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Inventory.Desktop.Commands;
using SideBarItem = Inventory.Desktop.Controls.SideBarItem;

namespace Inventory.Desktop.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ICommand CloseWindowCommand { get; }
        public ICommand ToggleFullScreenCommand { get; }
        public ICommand MinimizeCommand { get; }

        private List<SideBarItem> sideBarNav = new List<SideBarItem>();

        public MainWindowViewModel()
        {
            CloseWindowCommand = new RelayCommand(() => Application.Current.Shutdown());

            ToggleFullScreenCommand =
                new RelayCommand(() =>
                {
                    if (Application.Current.MainWindow == null)
                        return;

                    var current = Application.Current.MainWindow.WindowState;
                    Application.Current.MainWindow.WindowState =
                        current == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
                });

            MinimizeCommand =
                new RelayCommand(() =>
                {
                    if (Application.Current.MainWindow != null)
                        Application.Current.MainWindow.WindowState = WindowState.Minimized;
                });
        }
    }
}