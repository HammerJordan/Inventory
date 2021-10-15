using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Application.Core.Models.Record.Queries;
using Inventory.Application.Core.Models.Record.Commands;
using Inventory.Desktop.Commands;
using Inventory.Desktop.Events;
using Inventory.Desktop.PopupWindows;
using Inventory.Domain.Models;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PubSub;

namespace Inventory.Desktop.ViewModel
{
    public class SelectRecordWindowViewModel : ViewModelBase
    {
        private readonly IRecordModelQuery recordQuery;
        private readonly IMediator _mediator;
        private RecordModel selectedRecord;
        private readonly IServiceProvider _serviceProvider;
        public ObservableCollection<RecordModel> RecordsCollection { get; set; }
        public bool RenameActive { get; set; } = false;
        public RecordModel SelectedRecord
        {
            get => selectedRecord;
            set
            {
                SetProperty(ref selectedRecord, value);

                OnPropertyChanged(null);
            }
        }
        public Window OwnerWindow { get; set; }

        public ICommand CloseWindowCommand { get; set; }
        public ICommand OpenRecordCommand { get; }
        public ICommand AddNewRecordCommand { get; }
        public ICommand RenameRecordCommand { get; }
        public ICommand DeleteRecordCommand { get; }

        public SelectRecordWindowViewModel(IRecordModelQuery recordQuery, IMediator mediator, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            this.recordQuery = recordQuery;
            _mediator = mediator;
            var loadedInvoices = recordQuery.LoadAllAsync().Result;

            RecordsCollection = new ObservableCollection<RecordModel>();

            foreach (var record in loadedInvoices)
                RecordsCollection.Add(record);


            AddNewRecordCommand = new RelayCommand(AddNewRecord);
            DeleteRecordCommand = new RelayCommand(DeleteRecordAsync);
            OpenRecordCommand = new RelayCommand(OpenRecord);
            RenameRecordCommand = new RelayCommand(RenameRecord);
        }

        private void RenameRecord()
        {
            if (selectedRecord == null)
                return;

            var renameDialog = _serviceProvider.GetService<RenameDialogWindow>();
            renameDialog.ViewModel.CallBackOnWindowClose = s => { RenameRecord(SelectedRecord, s); };

            renameDialog.Owner = System.Windows.Application.Current.MainWindow;
            renameDialog.ShowDialog();


        }

        private void AddNewRecord()
        {
            var record = recordQuery.CreateAsync(new RecordModel()).Result;

            RecordsCollection.Insert(0, record);
            SelectedRecord = record;
        }

        private async void DeleteRecordAsync()
        {
            if (SelectedRecord == null)
                return;

            await _mediator.Send(new DeleteRecordCommand(SelectedRecord));

            int selectedIndex = RecordsCollection
                .IndexOf(RecordsCollection.First(x => x.ID == SelectedRecord.ID));

            RecordsCollection.RemoveAt(selectedIndex);

            if (RecordsCollection.Count == 0)
                SelectedRecord = null;
            else
                SelectedRecord = selectedIndex == 0 ?
                    RecordsCollection[0] :
                    RecordsCollection[selectedIndex - 1];

            OnPropertyChanged(null);
        }

        private void OpenRecord()
        {
            Hub.Default.Publish(new RecordModelSelectEvent(SelectedRecord));
            CloseWindowCommand.Execute(null);
        }

        private async Task RenameRecord(RecordModel recordModel, string newName)
        {
            recordModel.Name = newName;
            await recordQuery.UpdateAsync(recordModel);

            int recordIndex = RecordsCollection.IndexOf(recordModel);
            RecordsCollection.RemoveAt(recordIndex);
            RecordsCollection.Insert(recordIndex, recordModel);
            SelectedRecord = recordModel;
        }
    }
}