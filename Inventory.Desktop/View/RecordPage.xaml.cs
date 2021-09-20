using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Inventory.Desktop.ViewModel;

namespace Inventory.Desktop.View
{
    /// <summary>
    /// Interaction logic for RecordPage.xaml
    /// </summary>
    public partial class RecordPage : UserControl
    {
        private readonly RecordViewModel vm;

        public RecordPage(RecordViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
