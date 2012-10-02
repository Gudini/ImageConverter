using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace ImageConverter
{
    class NegativeThread
    {
        public delegate void NegDelMessage(BitmapImage image);

        public delegate void NegProcessBarValue(int value, int procent);

        private readonly NegDelMessage _delMessage;

        private readonly NegProcessBarValue _processBarValue;

        public NegativeThread(NegDelMessage delMessage, NegProcessBarValue processBarValue)
        {
            this._delMessage = delMessage;
            this._processBarValue = processBarValue;
        }

        public void StartNegative(BitmapImage image)
        {
            var bitmapConverter = new BitmapImageToBitmapConverter();
            var mConvertBitmap = (Bitmap)bitmapConverter.ConvertBack(image, null, null, null);

            int w_b = mConvertBitmap.Width;
            int h_b = mConvertBitmap.Height;

            int k = 1, maxValue = w_b;
            double curValue = 0;

            for (int x = 0; x < w_b; x++)
            {
                for (int y = 0; y < h_b; y++)
                {
                    Color c = mConvertBitmap.GetPixel(x, y);
                    mConvertBitmap.SetPixel(x, y, Color.FromArgb(255 - c.R, 255 - c.G, 255 - c.B));
                }
                //curValue = k / maxValue;
                //_processBarValue(k, (int)curValue);
                //k++;

            }

            var bitmapImageConverter = new BitmapImageToBitmapConverter();
            var mBTImage = (BitmapImage)bitmapImageConverter.Convert(mConvertBitmap, null, null, null);

            _delMessage(mBTImage);

            const string message = "Преобразование завершено!";
            const string caption = "Завершение";

            const MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBox.Show(message, caption, buttons);
        }
    }
}
