using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using InventoryManagement.Desktop.Commands;
using InventoryManagement.Desktop.Controls;

namespace InventoryManagement.Desktop.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ICommand CloseWindowCommand { get; }
        public ICommand ToggleFullScreenCommand { get; }
        public ICommand MinimizeCommand { get; }

        private List<SideBarItem> sideBarNav = new List<SideBarItem>();


        public MainWindowViewModel() 
        {
            CloseWindowCommand =
                new RelayCommand((o => true), c =>
                {
                    Application.Current.Shutdown();
                });

            ToggleFullScreenCommand =
                new RelayCommand((o => true), c =>
                {
                    if (Application.Current.MainWindow == null)
                        return;

                    var current = Application.Current.MainWindow.WindowState;
                    Application.Current.MainWindow.WindowState =
                        current == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
                });

            MinimizeCommand =
                new RelayCommand((o => true), c =>
                {
                    if (Application.Current.MainWindow != null)
                        Application.Current.MainWindow.WindowState = WindowState.Minimized;
                });


            


        }


    }
}