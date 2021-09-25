using System.Collections.ObjectModel;
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
        private string recordNameEdit = string.Empty;

        private bool editRecordName;

        private readonly InvoiceDBHelper invoiceDbHelper;
        private RecordBindableModel record;
        public ICommand OpenRecordCommand { get; }
        public ICommand EditRecordCommand { get; }
        public ICommand RenameRecordCommand { get; }
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
        public bool EditRecordName
        {
            get => editRecordName;
            set
            {
                SetProperty(ref editRecordName, value);
                OnPropertyChanged(nameof(RecordNameEmpty));
            }
        }
        public string RecordNameEdit 
        {
            get => recordNameEdit;
            set
            {
                SetProperty(ref recordNameEdit, value);
                OnPropertyChanged(nameof(RecordNameEmpty));
            }
        }

        public bool RecordNameEmpty => EditRecordName && string.IsNullOrEmpty(RecordNameEdit);

        public HomeViewModel(InvoiceDBHelper invoiceDbHelper)
        {
            this.invoiceDbHelper = invoiceDbHelper;
            ProductViewModels = new ObservableCollection<ProductViewModel>();

            var hub = Hub.Default;
            hub.Subscribe<ProductModelAddRemove>(ModifyProducts);

            OpenRecordCommand = new RelayCommand(_ => true, _ => OpenRecord());
            EditRecordCommand = new RelayCommand(() => EditRecordName = true);
            RenameRecordCommand = new RelayCommand(RenameRecord);

            Hub.Default.Subscribe<RecordModelSelect>(this, x => { Record = x.Record; });
        }

        private void RenameRecord()
        {
            if (Record == null)
                return;
            if (string.IsNullOrEmpty(RecordNameEdit))
                return;

            Record.Name = RecordNameEdit;
            RecordNameEdit = string.Empty;
            EditRecordName = false;

            invoiceDbHelper.SaveRecordModel(Record);
        }

        private void OpenRecord()
        {
            var window = IoC.Get<SelectRecordWindow>();
            window.Owner = Application.Current.MainWindow;
            window.ShowDialog();
        }

        public void ModifyProducts(ProductModelAddRemove model)
        {
            ProductViewModels.Add(new ProductViewModel { ProductModel = model.Model });
        }
    }
}