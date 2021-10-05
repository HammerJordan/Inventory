using System.Windows;
using Inventory.Desktop.Commands;
using Inventory.Desktop.ViewModel;

namespace Inventory.Desktop.PopupWindows
{
    /// <summary>
    ///     Interaction logic for SelectRecordWindow.xaml
    /// </summary>
    public partial class SelectRecordWindow : Window
    {
        public SelectRecordWindow(SelectRecordWindowViewModel vm)
        {
            InitializeComponent();
            vm.CloseWindowCommand = new RelayCommand(_ => Close());
            DataContext = vm;
        }
    }
}