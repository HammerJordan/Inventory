using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Inventory.DataAccess;
using InventoryManagement.Desktop.Commands;
using InventoryManagement.Desktop.Events;
using InventoryManagement.Desktop.Model;
using PubSub;

namespace InventoryManagement.Desktop.ViewModel
{
    public class RecordViewModel : ViewModelBase
    {
        private readonly InvoiceDBHelper invoiceDbHelper;
        private RecordBindableModel selectedRecord;

        public ICommand NewRecordCommand { get; }
        public ICommand DeleteRecordCommand { get; }
        public ObservableCollection<ProductViewModel> ProductViewModels { get; set; }
        public ObservableCollection<RecordBindableModel> RecordsCollection { get; set; }

        public RecordBindableModel SelectedRecord
        {
            get => selectedRecord;
            set
            {
                SetProperty(ref selectedRecord, value);

                OnPropertyChanged(null);
            }
        }

        public RecordViewModel(InvoiceDBHelper invoiceDbHelper)
        {
            this.invoiceDbHelper = invoiceDbHelper;

            var loadedInvoices = invoiceDbHelper.LoadInvoices();

            RecordsCollection = new ObservableCollection<RecordBindableModel>();
            foreach (RecordBindableModel record in loadedInvoices)
            {
                RecordsCollection.Add(record);
                record.PropertyChanged += RecordOnPropertyChanged;
            }

            ProductViewModels = new ObservableCollection<ProductViewModel>();

            var hub = Hub.Default;
            hub.Subscribe<ProductModelAddRemove>(ModifyProducts);

            NewRecordCommand = new RelayCommand((_) => true, (_) => AddNewRecord());
            DeleteRecordCommand = new RelayCommand((_) => true, (_) => DeleteRecord());
        }

        public void ModifyProducts(ProductModelAddRemove model)
        {
            ProductViewModels.Add(new ProductViewModel() { ProductModel = model.Model });
        }

        private void AddNewRecord()
        {
            var record = invoiceDbHelper.CreateNewInvoice();
            // TODO list view should set the selected invoice and this should overide that
            RecordBindableModel recordBindable = record;

            RecordsCollection.Insert(0, recordBindable);
            SelectedRecord = recordBindable;
        }

        private void RecordOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is not RecordBindableModel model)
                return;

            invoiceDbHelper.SaveRecordModel(model);
        }

        private void DeleteRecord()
        {
            if (SelectedRecord == null)
                return;
            invoiceDbHelper.DeleteRecordModel(SelectedRecord);

            int selectedIndex = RecordsCollection
                .IndexOf(RecordsCollection.First(x => x.ID == SelectedRecord.ID));

            RecordsCollection.RemoveAt(selectedIndex);

            if (RecordsCollection.Count == 0)
                SelectedRecord = null;
            else
            {
                if (selectedIndex == 0)
                    SelectedRecord = RecordsCollection[0];
                else
                    SelectedRecord = RecordsCollection[selectedIndex - 1];
            }

            OnPropertyChanged(null);
        }
    }
}