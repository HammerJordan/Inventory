using System.Windows.Input;
using Inventory.Desktop.Commands;
using Inventory.Desktop.Events;
using Inventory.Domain.Models;
using PubSub;

namespace Inventory.Desktop.ViewModel
{
    public class ProductViewModel : ViewModelBase
    {
        private ProductModel productModel;

        public int Quantity { get; set; } = 1;

        public ICommand IncrementQuantityCommand { get; }
        public ICommand DecrementQuantityCommand { get; }
        public ICommand AddToRecordCommand { get; }

        public ProductModel ProductModel
        {
            get => productModel;
            set => SetProperty(ref productModel, value);
        }

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
    }
}