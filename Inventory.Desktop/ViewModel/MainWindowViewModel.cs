using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Inventory.Desktop.Commands;
using Inventory.Desktop.Controls;
using Serilog;

namespace Inventory.Desktop.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private List<SideBarItem> sideBarNav = new();
        public ICommand CloseWindowCommand { get; }
        public ICommand ToggleFullScreenCommand { get; }
        public ICommand MinimizeCommand { get; }

        public MainWindowViewModel()
        {
            CloseWindowCommand = new RelayCommand(() => System.Windows.Application.Current.Shutdown());
            
            ToggleFullScreenCommand =
                new RelayCommand(() =>
                {
                    if (System.Windows.Application.Current.MainWindow == null)
                        return;

                    var current = System.Windows.Application.Current.MainWindow.WindowState;
                    System.Windows.Application.Current.MainWindow.WindowState =
                        current == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
                });

            MinimizeCommand =
                new RelayCommand(() =>
                {
                    if (System.Windows.Application.Current.MainWindow != null)
                        System.Windows.Application.Current.MainWindow.WindowState = WindowState.Minimized;
                });
        }
    }
}