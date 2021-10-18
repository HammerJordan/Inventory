using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Application.Core.Common.Interfaces;
using Application.Core.Models.Record.Queries;
using Application.Core.Models.RecordProductList;
using Application.Core.Models.RecordProductList.Queries;
using Application.WPF.WebScraping.ProductUpdates;
using Inventory.Desktop.Commands;
using Inventory.Desktop.Events;
using Inventory.Desktop.PopupWindows;
using Inventory.Domain.Models;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PubSub;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global


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
        public ICommand OpenRemoteWindowCommand { get; }
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
            OpenRemoteWindowCommand = new RelayCommand(OpenRemoteWindow);

            Hub.Default.Subscribe<AddProductModelToRecordEvent>(AddProductModelToRecord);
            Hub.Default.Subscribe<RecordModelSelectEvent>(OpenNewRecord);

        }

        private void OpenRemoteWindow()
        {
            var window = _serviceProvider.GetService<RemoteWindow>();
            window.Owner = System.Windows.Application.Current.MainWindow;
            window.ShowDialog();
        }

        private void ExportRecord()
        {
            var window = _serviceProvider.GetService<ExportWindow>();
            window.Owner = System.Windows.Application.Current.MainWindow;
            window.ShowDialog();
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
            Record = null;
            ProductViewModels.Clear();
            _serviceProvider.GetService<MainWindowViewModel>().RecordSelected = false;

            var window = _serviceProvider.GetService<SelectRecordWindow>();
            if (window == null)
                return;
            window.Owner = System.Windows.Application.Current.MainWindow;
            window.ShowDialog();


        }



        private void ProductViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is not "Quantity")
                return;

            OnPropertyChanged(null);

            if (sender is ProductViewModel vm)
                _recordProductList.SetQuantity(vm.ProductModel, vm.Quantity);
        }

        private async Task AddProductModelToRecord(AddProductModelToRecordEvent notification)
        {
            if (Record is null || notification?.Model?.ProductModel is null)
                return;

            if (ProductViewModels.Any(x => x.ProductModel.ID == notification.Model.ProductModel.ID))
            {
                var productViewModel = ProductViewModels
                    .First(x => x.ProductModel.ID == notification.Model.ProductModel.ID);
                productViewModel.Quantity++;
            }
            else
            {
                var productModel = await _mediator.Send(new CheckProductForUpdatesCommand(notification.Model.ProductModel));

                var productViewModel = new ProductViewModel
                {
                    ProductModel = productModel,
                    Quantity = notification.Model.Quantity
                };

                _recordProductList.Add(productViewModel.ProductModel, productViewModel.Quantity);
                ProductViewModels.Add(productViewModel);

                productViewModel.PropertyChanged += ProductViewModelPropertyChanged;
            }

            OnPropertyChanged(null);
        }

        private async Task OpenNewRecord(RecordModelSelectEvent recordEvent)
        {
            if (recordEvent.Record is null)
                return;
            Record = recordEvent.Record;
            ProductViewModels.Clear();

            var recordItems = await _recordItemsQuery.LoadAllAsync(Record);
            var recordItemsArray = recordItems as RecordListItem[] ?? recordItems.ToArray();
            var updatedItems = new List<ProductModel>();

            foreach (var item in recordItemsArray)
            {
                var model = await _mediator.Send(new CheckProductForUpdatesCommand(item.ProductModel));
                updatedItems.Add(model);
            }



            foreach (var productViewModel in updatedItems
                .Select(product => new ProductViewModel
                {
                    ProductModel = product,
                    Quantity = recordItemsArray.First(x => x.ProductID == product.ID).Quantity
                }))
            {
                productViewModel.PropertyChanged += ProductViewModelPropertyChanged;
                ProductViewModels.Add(productViewModel);
            }

            _recordProductList = new RecordProductList(Record, ProductViewModels
                .Select(item => new RecordListItem(item.ProductModel, Record)
                { Quantity = item.Quantity })
                .ToList(), _mediator);

            _serviceProvider.GetService<MainWindowViewModel>().RecordSelected = true;

            OnPropertyChanged(null);
        }




    }
}