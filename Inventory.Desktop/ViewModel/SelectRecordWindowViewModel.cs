using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Application.Core.Models.Record.Queries;
using Inventory.Desktop.Commands;
using Inventory.Desktop.Events;
using Inventory.Domain.Models;
using PubSub;

namespace Inventory.Desktop.ViewModel
{
    public class SelectRecordWindowViewModel : ViewModelBase
    {
        private readonly IRecordModelQuery recordQuery;
        private RecordModel selectedRecord;
        public ObservableCollection<RecordModel> RecordsCollection { get; set; }

        public RecordModel SelectedRecord
        {
            get => selectedRecord;
            set
            {
                SetProperty(ref selectedRecord, value);

                OnPropertyChanged(null);
            }
        }

        public ICommand CloseWindowCommand { get; set; }
        public ICommand OpenRecordCommand { get; }
        public ICommand AddNewRecordCommand { get; }
        public ICommand DeleteRecordCommand { get; }

        public SelectRecordWindowViewModel(IRecordModelQuery recordQuery)
        {
            this.recordQuery = recordQuery;
            var loadedInvoices = recordQuery.LoadAllAsync().Result;

            RecordsCollection = new ObservableCollection<RecordModel>();

            foreach (var record in loadedInvoices)
                RecordsCollection.Add(record);
            

            AddNewRecordCommand = new RelayCommand(AddNewRecord);
            DeleteRecordCommand = new RelayCommand(DeleteRecord);
            OpenRecordCommand = new RelayCommand(OpenRecord);
        }

        private void AddNewRecord()
        {
            var record = recordQuery.CreateAsync(new RecordModel()).Result;

            RecordsCollection.Insert(0, record);
            SelectedRecord = record;
        }

        private void DeleteRecord()
        {
            if (SelectedRecord == null)
                return;

            recordQuery.DeleteAsync(SelectedRecord);

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
            Hub.Default.Publish(new RecordModelSelect(SelectedRecord));
            CloseWindowCommand.Execute(null);
        }
    }
}