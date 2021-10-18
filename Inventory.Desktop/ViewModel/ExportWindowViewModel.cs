using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Navigation;
using Application.Core.Common.Interfaces;
using Application.Core.Models.Record.Queries;
using Application.Core.Models.RecordProductList.Queries;
using Inventory.Desktop.Commands;
using Inventory.Domain.Models;

namespace Inventory.Desktop.ViewModel
{
    public class ExportWindowViewModel : ViewModelBase
    {
        private readonly IRecordModelQuery _recordQuery;
        private readonly IRecordListItemQuery _recordItemsQuery;
        private readonly IExportCsvFile _exportCsv;

        public string FilePath { get; set; }
        public string FileName { get; set; }

        public ICommand OpenSelectPathDialogCommand { get; set; }
        public ICommand ExportCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public bool AnyRecordSelected => Records.Any(x => x.ToExport);

        public Window OwnerWindow { get; set; }

        public ObservableCollection<RecordExportViewModel> Records { get; set; } = new ObservableCollection<RecordExportViewModel>();

        public ExportWindowViewModel(IRecordModelQuery recordQuery, IRecordListItemQuery recordItemsQuery, IExportCsvFile exportCsv)
        {
            _recordQuery = recordQuery;
            _recordItemsQuery = recordItemsQuery;
            _exportCsv = exportCsv;
            Setup();


            OpenSelectPathDialogCommand = new RelayCommand(() =>
            {
                using var dialog = new FolderBrowserDialog();
                dialog.ShowDialog();
                string path = dialog.SelectedPath;
                if (string.IsNullOrEmpty(path))
                    return;
                FilePath = path;
            });

            ExportCommand = new RelayCommand(
                (x) => !string.IsNullOrEmpty(FilePath) && !string.IsNullOrEmpty(FileName) && AnyRecordSelected,
                Export);

            CancelCommand = new RelayCommand(() => OwnerWindow.Close());
        }
        private async void Export(object obj)
        {
            await _exportCsv.ExportToCSV(FilePath, FileName, Records.Where(x => x.ToExport).Select(x => x.Record));
            CancelCommand.Execute(null);
        }


        private void Setup()
        {
            var records = _recordQuery.LoadAllAsync();

            foreach (var recordModel in records.Result)
            {
                var recordItemsCount = _recordItemsQuery.Count(recordModel);
                Records.Add(new RecordExportViewModel()
                {
                    Record = recordModel,
                    RecordItemsCount = recordItemsCount.Result
                });
            }

        }
    }


    public class RecordExportViewModel : ViewModelBase
    {
        public RecordModel Record { get; set; }
        public int RecordItemsCount { get; set; }
        public bool ToExport { get; set; }
    }
}