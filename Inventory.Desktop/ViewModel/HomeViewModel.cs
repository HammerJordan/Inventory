using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using Application.Common.Interfaces;
using Application.Models.Record.Queries;
using Application.Models.RecordProductList;
using Application.Models.RecordProductList.Queries;
using Inventory.Desktop.Commands;
using Inventory.Desktop.Events;
using Inventory.Desktop.PopupWindows;
using Inventory.Domain.Models;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PubSub;

namespace Inventory.Desktop.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly IExportCsvFile _exportRecord;
        private readonly IRecordListItemQuery _recordItemsQuery;
        private readonly IRecordModelQuery _recordQuery;
        private readonly IMediator _mediator;
        private readonly IServiceProvider _serviceProvider;

        private RecordProductList _recordProductList;

        public ICommand OpenRecordCommand { get; }
        public ICommand EditRecordCommand { get; }
        public ICommand RenameRecordCommand { get; }
        public ICommand DeleteRecordCommand { get; }
        public ICommand ExportCommand { get; }
        public ObservableCollection<ProductViewModel> ProductViewModels { get; set; }

        public RecordModel Record { get; set; }
        public bool EditRecordName { get; set; }
        public string RecordNameEdit { get; set; }

        public bool RecordNameEmpty => EditRecordName && string.IsNullOrEmpty(RecordNameEdit);

        public decimal Subtotal => ProductViewModels.Sum(x => x.Quantity * x.ProductModel.Cost);
        public int TotalItems => ProductViewModels.Sum(x => x.Quantity);

        public HomeViewModel(IExportCsvFile exportRecord,
            IRecordListItemQuery recordItemsQuery,
            IRecordModelQuery recordQuery,
            IMediator mediator,
            IServiceProvider serviceProvider)
        {
            _exportRecord = exportRecord;
            _recordItemsQuery = recordItemsQuery;
            _recordQuery = recordQuery;
            _mediator = mediator;
            _serviceProvider = serviceProvider;

            ProductViewModels = new ObservableCollection<ProductViewModel>();

            var hub = Hub.Default;
            hub.Subscribe<ProductModelAddRemove>(ModifyProducts);

            OpenRecordCommand = new RelayCommand(_ => true, _ => OpenRecord());
            EditRecordCommand = new RelayCommand(() => EditRecordName = true);
            RenameRecordCommand = new RelayCommand(RenameRecord);
            DeleteRecordCommand = new RelayCommand(x =>
            {
                if (x is not ProductViewModel vm)
                    return;
                DeleteRecord(vm);
            });

            ExportCommand = new RelayCommand(ExportRecord);

            Hub.Default.Subscribe<RecordModelSelect>(this, x =>
            {
                Record = x.Record;
                ProductViewModels.Clear();

                var recordItems = recordItemsQuery.LoadAllAsync(Record).Result;

                foreach (var product in recordItemsQuery.LoadAllAsync(Record).Result)
                {
                    var productViewModel = new ProductViewModel
                    {
                        ProductModel = product.ProductModel,
                        Quantity = product.Quantity
                    };
                    productViewModel.PropertyChanged += ProductViewModelPropertyChanged;
                    ProductViewModels.Add(productViewModel);
                }

                _recordProductList = new RecordProductList(Record, ProductViewModels
                    .Select(item => new RecordListItem(item.ProductModel, Record)
                        { Quantity = item.Quantity })
                    .ToList(), _mediator);

                OnPropertyChanged(null);
            });
        }

        private void ExportRecord()
        {
            using var dialog = new FolderBrowserDialog();
            var result = dialog.ShowDialog();
            string path = dialog.SelectedPath;

            _exportRecord.ExportToCSV(path, _recordProductList);
        }

        private void DeleteRecord(ProductViewModel vm)
        {
            ProductViewModels.Remove(vm);
            _recordProductList.Remove(vm.ProductModel);
            OnPropertyChanged(null);
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

            _recordQuery.UpdateAsync(Record);
            OnPropertyChanged(null);
        }

        private void OpenRecord()
        {
            var window = _serviceProvider.GetService<SelectRecordWindow>();
            window.Owner = System.Windows.Application.Current.MainWindow;
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

                // _recordItemsQuery.UpdateProduct(Record, productViewModel.ProductModel);
            }
            else
            {
                var productViewModel = new ProductViewModel { ProductModel = model.Model };
                productViewModel.PropertyChanged += ProductViewModelPropertyChanged;
                productViewModel.Quantity = productViewModel.Quantity;

                _recordProductList.Add(productViewModel.ProductModel, productViewModel.Quantity);
                ProductViewModels.Add(productViewModel);
            }

            OnPropertyChanged(null);
        }

        private void ProductViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is not "Quantity")
                return;

            OnPropertyChanged(null);
            
            if (sender is ProductViewModel vm)
                _recordProductList.SetQuantity(vm.ProductModel, vm.Quantity);
        }
    }
}