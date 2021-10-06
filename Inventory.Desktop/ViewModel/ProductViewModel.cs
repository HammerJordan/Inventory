using System.Windows.Input;
using Inventory.Desktop.Commands;
using Inventory.Desktop.Events;
using Inventory.Domain.Models;
using MediatR;
using PubSub;
using Serilog;

namespace Inventory.Desktop.ViewModel
{
    public class ProductViewModel : ViewModelBase
    {
        private ProductModel _productModel;

        public int Quantity { get; set; } = 1;

        public ICommand IncrementQuantityCommand { get; }
        public ICommand DecrementQuantityCommand { get; }
        public ICommand AddToRecordCommand { get; }

        public ProductModel ProductModel
        {
            get => _productModel;
            set => SetProperty(ref _productModel, value);
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
            Log.Debug("Add to products from Product VM");
            Hub.Default.Publish(new AddProductModelToRecordEvent(this));
        }
    }
}