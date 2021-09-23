using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Inventory.DataAccess;
using Inventory.Desktop.Commands;
using Inventory.Desktop.Events;
using Inventory.Desktop.Model;
using PubSub;

namespace Inventory.Desktop.ViewModel
{
    public class SelectRecordWindowViewModel : ViewModelBase
    {
        private readonly InvoiceDBHelper invoiceDbHelper;

        private RecordBindableModel selectedRecord;
        
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

        public ICommand CloseWindowCommand { get; set; }
        public ICommand OpenRecordCommand { get; set; }
        public ICommand AddNewRecordCommand { get; set; }
        public ICommand DeleteRecordCommand { get; set; }

        public SelectRecordWindowViewModel(InvoiceDBHelper invoiceDbHelper)
        {
            this.invoiceDbHelper = invoiceDbHelper;

            var loadedInvoices = invoiceDbHelper.LoadInvoices();

            RecordsCollection = new ObservableCollection<RecordBindableModel>();
            foreach (RecordBindableModel record in loadedInvoices)
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
            if (sender is not RecordBindableModel model)
                return;

            invoiceDbHelper.SaveRecordModel(model);
        }

        private void AddNewRecord()
        {
            var record = invoiceDbHelper.CreateNewInvoice();
            RecordBindableModel recordBindableModel = record;

            RecordsCollection.Insert(0, recordBindableModel);
            SelectedRecord = recordBindableModel;
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