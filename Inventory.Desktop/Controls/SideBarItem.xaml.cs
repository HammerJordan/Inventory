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
using System.Windows.Navigation;
using System.Windows.Shapes;
using InventoryManagement.Core.IoC;
using InventoryManagement.Desktop.Commands;
using InventoryManagement.Desktop.ViewModel;

namespace InventoryManagement.Desktop.Controls
{
    /// <summary>
    /// Interaction logic for SideBarItem.xaml
    /// </summary>
    public partial class SideBarItem : UserControl
    {

        public static readonly DependencyProperty SetNameText =
            DependencyProperty.Register("SideBarName", typeof(string), typeof(SideBarItem), new
                PropertyMetadata("", OnSideBarNameChanged));

        public static readonly DependencyProperty SetSideBarIcon =
            DependencyProperty.Register("SideBarIcon", typeof(string), typeof(SideBarItem), new
                PropertyMetadata("", OnIconChanged));

        public static readonly DependencyProperty SetSideBarIsSelected =
            DependencyProperty.Register("SideBarSelected", typeof(bool), typeof(SideBarItem), new
                PropertyMetadata(true, OnIsSelectedChanged));

        private static void OnSideBarNameChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var UserControl1Control = d as SideBarItem;
            UserControl1Control?.OnSetTextChanged(e);
        }

        private static void OnIconChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var UserControl1Control = d as SideBarItem;
            UserControl1Control?.OnIconChanged(e);
        }

        private static void OnIsSelectedChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var UserControl1Control = d as SideBarItem;
            UserControl1Control?.OnIsSelectedChanged(e);
        }

        public string SideBarName
        {
            get => (string)GetValue(SetNameText);
            set => SetValue(SetNameText, value);
        }

        public string SideBarIcon
        {
            get => (string)GetValue(SetSideBarIcon);
            set => SetValue(SetSideBarIcon, value);
        }

        public bool SideBarSelected
        {
            get => (bool)GetValue(SetSideBarIsSelected);
            set => SetValue(SetSideBarIsSelected, value);
        }

        private void OnSetTextChanged(DependencyPropertyChangedEventArgs e)
        {
            Name.Text = e.NewValue.ToString();
        }

        private void OnIconChanged(DependencyPropertyChangedEventArgs e)
        {
            Icon.Text = e.NewValue.ToString();
        }

        private void OnIsSelectedChanged(DependencyPropertyChangedEventArgs e)
        {
            //SelectedPip.Background = new SolidColorBrush();
            SelectedPip.Background = (bool)e.NewValue ? defaultSelectedPipBrush : new SolidColorBrush();
        }

        public event Action<SideBarItem> SideBarClickEvent;

        private readonly Brush defaultSelectedPipBrush;




        public SideBarItem()
        {
            InitializeComponent();
            defaultSelectedPipBrush = SelectedPip.Background;
        }

        private void OnSideBarClicked(object sender, RoutedEventArgs e)
        {
            SideBarClickEvent?.Invoke(this);
        }
    }
}
