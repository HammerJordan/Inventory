using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Inventory.Desktop.Controls
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
            SideBarSelected = (bool)e.NewValue;
            //SelectedPip.Background = new SolidColorBrush();
            //Name.Foreground = (bool)e.NewValue ? defaultTextColor : selectedTextBrush;
            //Icon.Foreground = (bool)e.NewValue ? selectedTextBrush : defaultTextColor;
        }

        public event Action<SideBarItem> SideBarClickEvent;





        public SideBarItem()
        {
            DataContext = this;
            InitializeComponent();
        }

        private void OnSideBarClicked(object sender, RoutedEventArgs e)
        {
            SideBarClickEvent?.Invoke(this);
        }

    }
}
