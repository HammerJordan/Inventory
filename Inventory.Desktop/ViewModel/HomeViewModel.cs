using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Mime;
using System.Windows;
using System.Windows.Input;
using Inventory.Core.IoC;
using Inventory.DataAccess;
using Inventory.Desktop.Commands;
using Inventory.Desktop.Events;
using Inventory.Desktop.Model;
using Inventory.Desktop.PopupWindows;
using PubSub;

namespace Inventory.Desktop.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly InvoiceDBHelper invoiceDbHelper;
        private RecordBindableModel record;
        public ICommand OpenRecordCommand { get; }
        public ObservableCollection<ProductViewModel> ProductViewModels { get; set; }

        public RecordBindableModel Record
        {
            get => record;
            set
            {
                SetProperty(ref record, value);
                OnPropertyChanged(null);
            }
        }

        public HomeViewModel(InvoiceDBHelper invoiceDbHelper)
        {
            this.invoiceDbHelper = invoiceDbHelper;

            ProductViewModels = new ObservableCollection<ProductViewModel>();

            var hub = Hub.Default;
            hub.Subscribe<ProductModelAddRemove>(ModifyProducts);

            OpenRecordCommand = new RelayCommand((_) => true, (_) => OpenRecord());

            Hub.Default.Subscribe<RecordModelSelect>(this, (x) =>
            {
                Record = x.Record;
            });
        }

        private void OpenRecord()
        {
            var window = IoC.Get<SelectRecordWindow>();
            window.Owner = Application.Current.MainWindow;
            window.ShowDialog();
        }



        public void ModifyProducts(ProductModelAddRemove model)
        {
            ProductViewModels.Add(new ProductViewModel() { ProductModel = model.Model });
        }



    }
}