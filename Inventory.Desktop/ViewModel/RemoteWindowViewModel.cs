using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Application.Core.Models.Record.Queries;
using Inventory.Desktop.Commands;
using Inventory.Domain.Models;
using QRCoder;

namespace Inventory.Desktop.ViewModel
{
    public class RemoteWindowViewModel : ViewModelBase
    {
        private RecordModel selectedRecord;
        private Process remoteProcessHandle;
        public string CommandName => remoteProcessHandle == null ? "Start Remote" : "Stop Remote";
        public ICommand StartStopCommand { get; private set; }
        public BitmapSource QrBitmap { get; private set; }
        public ObservableCollection<RecordModel> Records { get; set; }

        public RecordModel SelectedRecord
        {
            get => selectedRecord;
            set
            {
                if (selectedRecord == value)
                    return;
                SetProperty(ref selectedRecord,value);
                UpdateQrCode();
            }
        }

        public RemoteWindowViewModel(IRecordModelQuery recordModelQuery)
        {
            Records = new ObservableCollection<RecordModel>(recordModelQuery.LoadAllAsync().Result);
            StartStopCommand = new RelayCommand(StartStopRemote);
        }

        private void UpdateQrCode()
        {
            var generator = new QRCodeGenerator();
            var payload = new PayloadGenerator.Url($@"https://192.168.0.16:5000/{SelectedRecord.ID}");
            var data = generator.CreateQrCode(payload);

            var qrCode = new QRCode(data);

            var qrBitmap = qrCode.GetGraphic(10);

            QrBitmap = CreateBitmapSourceFromGdiBitmap(qrBitmap);
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

        private void StartStopRemote()
        {
            if (remoteProcessHandle == null)
            {
                remoteProcessHandle = new Process();
                remoteProcessHandle.StartInfo.FileName =
                    "C:\\Users\\Hammer\\Desktop\\MyProjects\\C#\\Inventory\\Inventory.Remote\\bin\\Release\\net5.0\\publish\\Inventory.Remote.exe";
                remoteProcessHandle.StartInfo.WorkingDirectory =
                    "C:\\Users\\Hammer\\Desktop\\MyProjects\\C#\\Inventory\\Inventory.Remote\\bin\\Release\\net5.0\\publish";
                remoteProcessHandle.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                remoteProcessHandle.Start();
            }
            else
            {
                remoteProcessHandle.Kill();
                remoteProcessHandle = null;
            }

        }

        public void StopProcess()
        {
            if (remoteProcessHandle != null)
                remoteProcessHandle.Kill(true);

            remoteProcessHandle = null;
        }



    }
}