using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Inventory.Core;
using Inventory.Core.IoC;
using Inventory.DataAccess;
using Inventory.Desktop.Commands;
using Inventory.Desktop.Events;
using Inventory.Desktop.PopupWindows;
using PubSub;

namespace Inventory.Desktop.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {

        private readonly IRecordQuery recordQuery;
        private readonly IRecordItemsQuery recordItemsQuery;
        public ICommand OpenRecordCommand { get; }
        public ICommand EditRecordCommand { get; }
        public ICommand RenameRecordCommand { get; }
        public ICommand DeleteRecordCommand { get; }
        public ObservableCollection<ProductViewModel> ProductViewModels { get; set; }
        public RecordModel Record { get; set; }
        public bool EditRecordName { get; set; }
        public string RecordNameEdit { get; set; }
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
            DeleteRecordCommand = new RelayCommand((x) =>
            {
                if (x is not ProductViewModel vm)
                    return;
                DeleteRecord(vm);
            });

            Hub.Default.Subscribe<RecordModelSelect>(this, x =>
            {
                Record = x.Record;
                ProductViewModels.Clear();

                foreach (var product in recordItemsQuery.LoadAll(Record))
                {
                    var productViewModel = new ProductViewModel { ProductModel = product, Quantity = product.Quantity};
                    productViewModel.PropertyChanged += ProductViewModelPropertyChanged;
                    ProductViewModels.Add(productViewModel);
                }


                OnPropertyChanged(null);
            });
        }

        private void DeleteRecord(ProductViewModel vm)
        {
            ProductViewModels.Remove(vm);
            OnPropertyChanged(null);
            recordItemsQuery.Delete(Record,vm.ProductModel);
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
            OnPropertyChanged(null);
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

            if (ProductViewModels.Any(x => x.ProductModel.ID == model.Model.ID))
            {
                var productViewModel = ProductViewModels.First(x => x.ProductModel.ID == model.Model.ID);
                productViewModel.Quantity++;
                productViewModel.ProductModel.Quantity = productViewModel.Quantity;
                recordItemsQuery.UpdateProduct(Record, productViewModel.ProductModel);
            }
            else
            {
                var productViewModel = new ProductViewModel { ProductModel = model.Model };
                productViewModel.PropertyChanged += ProductViewModelPropertyChanged;
                productViewModel.ProductModel.Quantity = productViewModel.Quantity;
                recordItemsQuery.InsertProduct(Record, productViewModel.ProductModel);
                ProductViewModels.Add(productViewModel);
            }


            OnPropertyChanged(null);
        }

        private void ProductViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName is not "Quantity")
                return;


            OnPropertyChanged(null);
            if (sender is ProductViewModel vm)
            {
                vm.ProductModel.Quantity = vm.Quantity;
                recordItemsQuery.UpdateProduct(Record,vm.ProductModel);
            }

        }
    }
}