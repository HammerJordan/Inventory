using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using Inventory.Desktop.Events;
using Inventory.Desktop.ViewModel;
using PubSub;

namespace Inventory.Desktop.Controls
{
    /// <summary>
    ///     Interaction logic for ProductItem.xaml
    /// </summary>
    public partial class ProductItem : UserControl
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(ProductViewModel), typeof(ProductItem),
            new PropertyMetadata(default(ProductViewModel)));

        public static readonly DependencyProperty ActionForegroundProperty = DependencyProperty.Register(
            "ActionForeground", typeof(Brush), typeof(ProductItem), new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty ActionBackgroundProperty = DependencyProperty.Register(
            "ActionBackground", typeof(Brush), typeof(ProductItem), new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty ActionWidthProperty = DependencyProperty.Register(
            "ActionWidth", typeof(double), typeof(ProductItem), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty ActionContentProperty = DependencyProperty.Register(
            "ActionContent", typeof(string), typeof(ProductItem), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ActionCommandProperty = DependencyProperty.Register(
            "ActionCommand", typeof(ICommand), typeof(ProductItem), new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty ActionCommandPramProperty = DependencyProperty.Register(
            "ActionCommandPram", typeof(object), typeof(ProductItem), new PropertyMetadata(default(object)));

        public ProductViewModel ViewModel
        {
            get => (ProductViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        public Brush ActionForeground
        {
            get => (Brush)GetValue(ActionForegroundProperty);
            set => SetValue(ActionForegroundProperty, value);
        }

        public Brush ActionBackground
        {
            get => (Brush)GetValue(ActionBackgroundProperty);
            set => SetValue(ActionBackgroundProperty, value);
        }

        public double ActionWidth
        {
            get => (double)GetValue(ActionWidthProperty);
            set => SetValue(ActionWidthProperty, value);
        }

        public string ActionContent
        {
            get => (string)GetValue(ActionContentProperty);
            set => SetValue(ActionContentProperty, value);
        }

        public ICommand ActionCommand
        {
            get => (ICommand)GetValue(ActionCommandProperty);
            set => SetValue(ActionCommandProperty, value);
        }

        public object ActionCommandPram
        {
            get => GetValue(ActionCommandPramProperty);
            set => SetValue(ActionCommandPramProperty, value);
        }

        public ProductItem()
        {
            InitializeComponent();
            DataContext = this;
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
            var regex = new Regex("[^0-9]+");
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