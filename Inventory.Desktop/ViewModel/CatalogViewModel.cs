using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Inventory.Application.Core.Models.Product.Commands;
using Inventory.Desktop.Commands;
using Inventory.Desktop.Events;
using MediatR;
using PubSub;

namespace Inventory.Desktop.ViewModel
{
    public class CatalogViewModel : ViewModelBase
    {
        private readonly IMediator _mediator;
        private CancellationTokenSource _cancellationToken;
        private string _searchBox;
        public ObservableCollection<ProductViewModel> ProductViewModels { get; set; }
        public ICommand AddToRecordCommand { get; }

        public string SearchBox
        {
            get => _searchBox;
            set
            {
                SetProperty(ref _searchBox, value);
                _ = UpdateSearchResults(value);
            }
        }
        
        

        public CatalogViewModel(IMediator mediator)
        {
            ProductViewModels = new ObservableCollection<ProductViewModel>();
            _mediator = mediator;

            AddToRecordCommand = new RelayCommand(  AddProductToCurrentRecord);
        }

        private void AddProductToCurrentRecord(object obj)
        {
            if (obj is not ProductViewModel vm)
                return;

            Hub.Default.Publish(new AddProductModelToRecordEvent(vm));
        }


        private async Task UpdateSearchResults(string newValue)
        {
            ProductViewModels.Clear();

            if (_cancellationToken is { Token: { CanBeCanceled: true } })
                _cancellationToken.Cancel();

            _cancellationToken = new CancellationTokenSource();

            var searchCommand = new ProductSearchCommand(newValue);

            var models = await _mediator.Send(searchCommand, _cancellationToken.Token);

            if (models == null)
                return;

            foreach (var model in models)
                ProductViewModels.Add(new ProductViewModel { ProductModel = model });
        }
    }
}