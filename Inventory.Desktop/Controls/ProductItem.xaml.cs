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
using Inventory.Desktop.Events;
using Inventory.Desktop.ViewModel;
using Inventory.Core.IoC;
using PubSub;

namespace Inventory.Desktop.Controls
{
    /// <summary>
    /// Interaction logic for ProductItem.xaml
    /// </summary>
    public partial class ProductItem : UserControl
    {

        //public static readonly DependencyProperty SetNameText =
        //    DependencyProperty.Register("SideBarName", typeof(string), typeof(SideBarItem), new
        //        PropertyMetadata("", OnSideBarNameChanged));

        //private static void OnSideBarNameChanged(DependencyObject d,
        //    DependencyPropertyChangedEventArgs e)
        //{
        //    var UserControl1Control = d as SideBarItem;
        //    UserControl1Control?.OnSetTextChanged(e);
        //}

        //public string SideBarName
        //{
        //    get => (string)GetValue(SetNameText);
        //    set => SetValue(SetNameText, value);
        //}

        //private void OnSetTextChanged(DependencyPropertyChangedEventArgs e)
        //{
        //    Name.Text = e.NewValue.ToString();
        //}

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
