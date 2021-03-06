using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Inventory.Desktop.ViewModel;

namespace Inventory.Desktop.PopupWindows
{
    /// <summary>
    /// Interaction logic for RemoteWindow.xaml
    /// </summary>
    public partial class RemoteWindow : Window
    {
        public RemoteWindow(RemoteWindowViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void CloseButtonOnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
