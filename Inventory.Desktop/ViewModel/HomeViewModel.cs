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
        private readonly IRecordItemsQuery recordItemsQuery;
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
        public int TotalItems => ProductViewModels.Sum(x => x.Quantity);

        public HomeViewModel(IRecordQuery recordQuery, IRecordItemsQuery recordItemsQuery)
        {
            this.recordQuery = recordQuery;
            this.recordItemsQuery = recordItemsQuery;
            ProductViewModels = new ObservableCollection<ProductViewModel>();

            var hub = Hub.Default;
            hub.Subscribe<ProductModelAddRemove>(ModifyProducts);

            OpenRecordCommand = new RelayCommand(_ => true, _ => OpenRecord());
            EditRecordCommand = new RelayCommand(() => EditRecordName = true);
            RenameRecordCommand = new RelayCommand(RenameRecord);

            Hub.Default.Subscribe<RecordModelSelect>(this, x =>
            {
                Record = x.Record;
                ProductViewModels.Clear();

                foreach (var product in recordItemsQuery.LoadAll(Record))
                    ModifyProducts(new ProductModelAddRemove(product));


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
            if (Record is null)
                return;

            var productModel = model.Model;
            if (ProductViewModels.Any(x => x.ProductModel.ID == productModel.ID))
                ProductViewModels.First(x => x.ProductModel.ID == productModel.ID).Quantity++;
            else
            {
                var productViewModel = new ProductViewModel { ProductModel = model.Model };
                productViewModel.PropertyChanged += ProductViewModelPropertyChanged;
                productViewModel.ProductModel.Quantity = productViewModel.Quantity;
                recordItemsQuery.UpdateProduct(Record, productViewModel.ProductModel);
                ProductViewModels.Add(productViewModel);
            }


            OnPropertyChanged(null);
        }

        private void ProductViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName is "Quantity")
                OnPropertyChanged(null);
        }
    }
}