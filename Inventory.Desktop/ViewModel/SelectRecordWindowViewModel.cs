using System.Collections.ObjectModel;
using System.Windows.Input;
using Inventory.Desktop.Model;

namespace Inventory.Desktop.ViewModel
{
    public class SelectRecordWindowViewModel : ViewModelBase
    {
        private RecordBindableModel selectedRecord;
        public ICommand CloseWindowCommand { get; set; }
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

        public ICommand AddNewRecord { get; set; }



    }
}