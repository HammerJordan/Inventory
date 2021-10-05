namespace Inventory.Desktop.Converters
{
    // public class ImageToSourceConverter : IValueConverter
    // {
    //     public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //     {
    //         if (value == null)
    //             return null;
    //
    //         var bitmap = new Bitmap((Image) value);
    //
    //         MemoryStream ms = new MemoryStream();
    //         (bitmap).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
    //         BitmapImage image = new BitmapImage();
    //         image.BeginInit();
    //         ms.Seek(0, SeekOrigin.Begin);
    //         image.StreamSource = ms;
    //         image.EndInit();
    //
    //         return image;
    //     }
    //     
    //
    //     public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //     {
    //         throw new NotImplementedException();
    //     }
    // }
}