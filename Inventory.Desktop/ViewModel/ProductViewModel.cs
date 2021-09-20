using Inventory.Core;

namespace Inventory.Desktop.ViewModel
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