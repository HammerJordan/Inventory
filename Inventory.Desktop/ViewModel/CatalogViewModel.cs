using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Inventory.DataAccess;
using Inventory.Desktop.Commands;
using Inventory.Desktop.Events;
using PubSub;

namespace Inventory.Desktop.ViewModel
{
    public class CatalogViewModel : ViewModelBase
    {
        public ObservableCollection<ProductViewModel> ProductViewModels { get; set; }
        public ICommand AddToRecordCommand { get; }

        private string searchBox;

        private readonly ProductSearchEngine searchEngine;
        private CancellationTokenSource cancellationToken;


        public string SearchBox
        {
            get => searchBox;
            set
            {
                SetProperty(ref searchBox, value);
                UpdateSearchResults(value);
            }
        }


        public CatalogViewModel(ProductSearchEngine searchEngine)
        {
            ProductViewModels = new ObservableCollection<ProductViewModel>();
            this.searchEngine = searchEngine;

            AddToRecordCommand = new RelayCommand((x) =>
            {
                if (x is not ProductViewModel vm)
                    return;

                Hub.Default.Publish(new ProductModelAddRemove(vm.ProductModel));
            });
        }

        private async Task UpdateSearchResults(string newValue)
        {
            ProductViewModels.Clear();

            if (cancellationToken != null && cancellationToken.Token.CanBeCanceled)
                cancellationToken.Cancel();

            cancellationToken = new CancellationTokenSource();
            
            
            var models = await Task.Run(() => searchEngine.SearchResults(newValue), cancellationToken.Token);
            
            if (models == null)
                return;
            
            foreach (var model in models)
            {
                ProductViewModels.Add(new ProductViewModel() { ProductModel = model });
            }

        }
    }
}