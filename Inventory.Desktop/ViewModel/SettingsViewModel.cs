using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Application.Core.Models.Product.Queries;
using Application.WPF.WebScraping.Common;
using Inventory.Desktop.Commands;
using QRCoder;
using PixelFormat = System.Windows.Media.PixelFormat;

namespace Inventory.Desktop.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly IProductUpdateRunner _productUpdateRunner;

        private double progressBar;

        public ICommand UpdateDatabaseCommand { get; }
        public ICommand StartRemoteCommand { get; }

        public BitmapSource QrBitmap { get; }

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

            StartRemoteCommand = new RelayCommand(StartRemote);


            QRCodeGenerator generator = new QRCodeGenerator();

            var gen = new PayloadGenerator.Url(@"https://192.168.0.16:45456/4");

            QRCodeData data = generator.CreateQrCode(gen);

            var qrCode = new QRCode(data);

            var qrBitmap = qrCode.GetGraphic(10);

            var x = CreateBitmapSourceFromGdiBitmap(qrBitmap);

            QrBitmap = x;

        }

        public static BitmapSource CreateBitmapSourceFromGdiBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException("bitmap");

            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            var bitmapData = bitmap.LockBits(
                rect,
                ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            try
            {
                var size = (rect.Width * rect.Height) * 4;

                return BitmapSource.Create(
                    bitmap.Width,
                    bitmap.Height,
                    bitmap.HorizontalResolution,
                    bitmap.VerticalResolution,
                    PixelFormats.Bgra32,
                    null,
                    bitmapData.Scan0,
                    size,
                    bitmapData.Stride);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }

        private void StartRemote()
        {
            Process prc = new Process();
            prc.StartInfo.FileName =
                "C:\\Users\\Hammer\\Desktop\\MyProjects\\C#\\Inventory\\Inventory.Remote\\bin\\Release\\net5.0\\publish\\Inventory.Remote.exe";
            prc.StartInfo.UseShellExecute = true;
            prc.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
            prc.Start();
        }

        private void CallBackUpdate()
        {
            ProgressBar = _productUpdateRunner.PercentDone * 100;

            if (_productUpdateRunner.IsDone)
                ProgressBar = 100;
        }
    }
}