using System.Threading.Tasks;
using System.Windows.Input;
using Application.Core.Models.Product.Queries;
using Application.WPF.WebScraping.Common;
using Inventory.Desktop.Commands;

namespace Inventory.Desktop.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly IProductUpdateRunner _productUpdateRunner;

        private double progressBar;

        public ICommand UpdateDatabaseCommand { get; }

        public double ProgressBar
        {
            get => progressBar;
            set => SetProperty(ref progressBar, value);
        }

        public SettingsViewModel(IProductUpdateRunner databaseUpdate)
        {
            _productUpdateRunner = databaseUpdate;

            UpdateDatabaseCommand = new RelayCommand(x => true, async x =>
            {
                ProgressBar = 0;
                await databaseUpdate.RunProductUpdateAsync(CallBackUpdate);
            });
        }

        private void CallBackUpdate()
        {
            ProgressBar = _productUpdateRunner.PercentDone * 100;
            
            if(_productUpdateRunner.IsDone)
                ProgressBar = 100;
        }
    }
}