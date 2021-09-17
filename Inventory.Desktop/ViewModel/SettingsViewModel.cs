using System.Threading.Tasks;
using System.Windows.Input;
using InventoryManagement.Desktop.Commands;
using WebScraping;

namespace InventoryManagement.Desktop.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly ProductUpdateRunner databaseUpdate;

        public ICommand UpdateDatabaseCommand { get; }

        private double progressBar;

        public double ProgressBar
        {
            get => progressBar;
            set => SetProperty(ref progressBar, value);
        }

        public SettingsViewModel(ProductUpdateRunner databaseUpdate)
        {
            this.databaseUpdate = databaseUpdate;

            UpdateDatabaseCommand = new RelayCommand(x => true, async x =>
            {
                ProgressBar = 0;
                var job = databaseUpdate.RunProductUpdate();

                while (!job.IsCompleted)
                {
                    ProgressBar = databaseUpdate.EstPercentDone * 100;
                    await Task.Delay(5000);
                }

                ProgressBar = 100;
            });
        }
    }
}