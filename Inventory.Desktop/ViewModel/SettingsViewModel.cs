using System.Threading.Tasks;
using System.Windows.Input;
using Inventory.Desktop.Commands;
using WebScraping;

namespace Inventory.Desktop.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly ProductUpdateRunner databaseUpdate;

        private double progressBar;

        public ICommand UpdateDatabaseCommand { get; }

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