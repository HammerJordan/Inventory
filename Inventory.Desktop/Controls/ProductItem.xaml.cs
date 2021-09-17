using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using InventoryManagement.Core.IoC;
using InventoryManagement.Desktop.Events;
using InventoryManagement.Desktop.ViewModel;
using PubSub;

namespace InventoryManagement.Desktop.Controls
{
    /// <summary>
    /// Interaction logic for ProductItem.xaml
    /// </summary>
    public partial class ProductItem : UserControl
    {
        public ProductItem()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            var myProcess = new Process
            {
                StartInfo =

            {
                UseShellExecute = true,
                FileName = e.Uri.AbsoluteUri
            }
            };
            myProcess.Start();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void AddButtonClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (!(DataContext is ProductViewModel model))
                return;

            var hub = Hub.Default;


            hub.Publish(new ProductModelAddRemove(model.ProductModel));
        }
    }
}
