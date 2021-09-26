using System.Windows.Input;
using Inventory.Core;
using Inventory.Desktop.Commands;
using Inventory.Desktop.Events;
using PubSub;

namespace Inventory.Desktop.ViewModel
{
    public class ProductViewModel : ViewModelBase
    {
        private ProductModel productModel;
        private int quantity = 1;

        public ICommand IncrementQuantityCommand { get; }
        public ICommand DecrementQuantityCommand { get; }
        public ICommand AddToRecordCommand { get; }

        public ProductViewModel()
        {
            IncrementQuantityCommand = new RelayCommand(() => Quantity++);
            DecrementQuantityCommand = new RelayCommand(() =>
            {
                if (Quantity > 1)
                    Quantity--;
            });
            AddToRecordCommand = new RelayCommand(AddProductToRecord);
        }

        private void AddProductToRecord()
        {
            Hub.Default.Publish(new ProductModelAddRemove(ProductModel));
        }

        public ProductModel ProductModel
        {
            get => productModel;
            set => SetProperty(ref productModel, value);
        }

        public int Quantity
        {
            get => quantity;
            set => SetProperty(ref quantity, value);
        }

    }
}