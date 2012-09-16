using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ImageConverter
{
    class BitmapImageToBitmapConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            MemoryStream ms = new MemoryStream();
            ((Bitmap) value).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();

            image.Freeze();

            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            MemoryStream ms = new MemoryStream();
            BitmapEncoder enc = new BmpBitmapEncoder();
            enc.Frames.Add(BitmapFrame.Create((BitmapImage)value));
            enc.Save(ms);
            Bitmap bitmap = new Bitmap(ms);

            return new Bitmap(bitmap);
        }
    }
}
