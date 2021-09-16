using Inventory.Mobile.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Inventory.Mobile.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}