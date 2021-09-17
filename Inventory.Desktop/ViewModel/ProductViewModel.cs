using Inventory.Core;

namespace InventoryManagement.Desktop.ViewModel
{
    public class ProductViewModel : ViewModelBase
    {
        private ProductModel productModel;

        public ProductModel ProductModel
        {
            get => productModel;
            set => SetProperty(ref productModel, value);
        }
    }
}