using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Application.Core.Common.Exceptions;
using Application.Core.Models.Product.Queries;
using Application.WPF.WebScraping.Common;
using Inventory.Desktop.Commands;
using Inventory.Desktop.PopupWindows;
using QRCoder;
using Microsoft.Extensions.DependencyInjection;
using PixelFormat = System.Windows.Media.PixelFormat;

namespace Inventory.Desktop.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly IProductUpdateRunner _productUpdateRunner;
        private readonly IServiceProvider _serviceProvider;

        private double progressBar;

        public ICommand UpdateDatabaseCommand { get; }
        public ICommand OpenRemoteWindowCommand { get; }


        public double ProgressBar
        {
            get => progressBar;
            set => SetProperty(ref progressBar, value);
        }

        public SettingsViewModel(IProductUpdateRunner databaseUpdate, IServiceProvider serviceProvider)
        {
            _productUpdateRunner = databaseUpdate;
            _serviceProvider = serviceProvider;

            UpdateDatabaseCommand = new RelayCommand(x => true, async x =>
            {
                ProgressBar = 0;
                await databaseUpdate.RunProductUpdateAsync(CallBackUpdate);
            });

            OpenRemoteWindowCommand = new RelayCommand(OpenRemoteWindow);
        }

        

        private void OpenRemoteWindow()
        {
            var remoteWindow = _serviceProvider.GetService<RemoteWindow>();
            if (remoteWindow == null)
                throw new NotFoundException("Remote Window cant be found");

            remoteWindow.Owner = System.Windows.Application.Current.MainWindow;
            remoteWindow.ShowDialog();



        }

        private void CallBackUpdate()
        {
            ProgressBar = _productUpdateRunner.PercentDone * 100;

            if (_productUpdateRunner.IsDone)
                ProgressBar = 100;
        }
    }
}