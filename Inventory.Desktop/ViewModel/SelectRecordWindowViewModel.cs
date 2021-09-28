using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Inventory.Core;
using Inventory.DataAccess;
using Inventory.Desktop.Commands;
using Inventory.Desktop.Events;
using Inventory.Desktop.Model;
using PubSub;

namespace Inventory.Desktop.ViewModel
{
    public class SelectRecordWindowViewModel : ViewModelBase
    {
        private readonly IRecordQuery recordQuery;

        private RecordViewModel selectedRecord;
        public ObservableCollection<RecordViewModel> RecordsCollection { get; set; }
        public RecordViewModel SelectedRecord
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

        public SelectRecordWindowViewModel(IRecordQuery recordQuery)
        {
            this.recordQuery = recordQuery;
            var loadedInvoices = recordQuery.LoadAll();

            RecordsCollection = new ObservableCollection<RecordViewModel>();
            
            foreach (RecordViewModel record in loadedInvoices)
            {
                RecordsCollection.Add(record);
                record.PropertyChanged += RecordOnPropertyChanged;
            }

            AddNewRecordCommand = new RelayCommand(AddNewRecord);
            DeleteRecordCommand = new RelayCommand(DeleteRecord);
            OpenRecordCommand = new RelayCommand(OpenRecord);
        }



        private void RecordOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is not RecordViewModel model)
                return;

            recordQuery.Update(model);
        }

        private void AddNewRecord()
        {
            var record = recordQuery.Create();
            RecordViewModel recordViewModel = record;

            RecordsCollection.Insert(0, recordViewModel);
            SelectedRecord = recordViewModel;
        }
        private void DeleteRecord()
        {
            if (SelectedRecord == null)
                return;

            recordQuery.Delete(SelectedRecord);

            int selectedIndex = RecordsCollection
                .IndexOf(RecordsCollection.First(x => x.ID == SelectedRecord.ID));

            RecordsCollection.RemoveAt(selectedIndex);

            if (RecordsCollection.Count == 0)
                SelectedRecord = null;
            else
            {
                SelectedRecord = selectedIndex == 0 ?
                    RecordsCollection[0] :
                    RecordsCollection[selectedIndex - 1];
            }

            OnPropertyChanged(null);

        }
        private void OpenRecord()
        {
            Hub.Default.Publish(new RecordModelSelect(SelectedRecord));
            CloseWindowCommand.Execute(null);
        }
    }
}