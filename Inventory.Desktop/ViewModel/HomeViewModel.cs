using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Inventory.Core;
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

        private readonly IRecordQuery recordQuery;
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

        public decimal Subtotal => ProductViewModels.Sum(x => x.Quantity * x.ProductModel.Cost);

        public HomeViewModel(IRecordQuery recordQuery)
        {
            this.recordQuery = recordQuery;
            ProductViewModels = new ObservableCollection<ProductViewModel>();

            var hub = Hub.Default;
            hub.Subscribe<ProductModelAddRemove>(ModifyProducts);

            OpenRecordCommand = new RelayCommand(_ => true, _ => OpenRecord());
            EditRecordCommand = new RelayCommand(() => EditRecordName = true);
            RenameRecordCommand = new RelayCommand(RenameRecord);

            Hub.Default.Subscribe<RecordModelSelect>(this, x =>
            {
                Record = x.Record;
                //TODO load the record items
                ProductViewModels.Clear();
                OnPropertyChanged(nameof(Subtotal));
            });
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

            recordQuery.Update(Record);
        }

        private void OpenRecord()
        {
            var window = IoC.Get<SelectRecordWindow>();
            window.Owner = Application.Current.MainWindow;
            window.ShowDialog();
        }

        public void ModifyProducts(ProductModelAddRemove model)
        {
            var productModel = model.Model;
            if (ProductViewModels.Any(x => x.ProductModel.ID == productModel.ID))
                ProductViewModels.First(x => x.ProductModel.ID == productModel.ID).Quantity++;
            else
                ProductViewModels.Add(new ProductViewModel { ProductModel = model.Model });

            OnPropertyChanged(nameof(Subtotal));
        }
    }
}