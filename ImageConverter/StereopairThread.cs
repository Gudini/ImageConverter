using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace ImageConverter
{
    class StereopairThread
    {
        public delegate void StereoMessage(BitmapImage image);

        private readonly StereoMessage _stMessage;

        public StereopairThread(StereoMessage stereoMessage)
        {
            _stMessage = stereoMessage;
        }

        public void StartConvertion(BitmapImage imageFirst, BitmapImage imageSecond)
        {
            var bitmapConvertionFirst = new BitmapImageToBitmapConverter();
            var mConvertBitmapFirst = (Bitmap) bitmapConvertionFirst.ConvertBack(imageFirst, null, null, null);

            var bitmapConvertionSecond = new BitmapImageToBitmapConverter();
            var mConvertBitmapSecond = (Bitmap)bitmapConvertionSecond.ConvertBack(imageSecond, null, null, null);

            int w_b_f = mConvertBitmapFirst.Width;
            int h_b_f = mConvertBitmapFirst.Height;

            int w_b_s = mConvertBitmapSecond.Width;
            int h_b_s = mConvertBitmapSecond.Height;

            var mColorAnaglyphsImage = new Bitmap(w_b_f, h_b_s);
            for (int i = 0; i < w_b_f; i++)
            {
                for(int j=0; j<h_b_f; j++)
                {
                    Color colorFirst = mConvertBitmapFirst.GetPixel(i, j);
                    Color colorSecond = mConvertBitmapSecond.GetPixel(i, j);
                    mColorAnaglyphsImage.SetPixel(i,j, Color.FromArgb((int) colorFirst.R, (int) colorSecond.G, (int) colorSecond.B));
                }
            }

            var mHalfColorAnaglyphsImage = new Bitmap(w_b_f, h_b_s);
            for (int i = 0; i < w_b_f; i++)
            {
                for (int j = 0; j < h_b_f; j++)
                {
                    Color colorFirst = mConvertBitmapFirst.GetPixel(i, j);
                    Color colorSecond = mConvertBitmapSecond.GetPixel(i, j);
                    mHalfColorAnaglyphsImage.SetPixel(i, j, Color.FromArgb((int)(0.299 * colorFirst.R + 0.587 * colorFirst.R + 0.114 * colorFirst.R), (int)colorSecond.G, (int)colorSecond.B));
                }
            }

            var mOptimizedAnaglyphsImage = new Bitmap(w_b_f, h_b_s);
            for (int i = 0; i < w_b_f; i++)
            {
                for (int j = 0; j < h_b_f; j++)
                {
                    Color colorFirst = mColorAnaglyphsImage.GetPixel(i, j);
                    Color colorSecond = mHalfColorAnaglyphsImage.GetPixel(i, j);
                    mOptimizedAnaglyphsImage.SetPixel(i, j, Color.FromArgb((int)(0.7 * colorFirst.R + 0.3 * colorFirst.R), (int)colorSecond.G, (int)colorSecond.B));
                }
            }

            var bitmapImageConverter = new BitmapImageToBitmapConverter();
            var mBtImage = (BitmapImage)bitmapImageConverter.Convert(mOptimizedAnaglyphsImage, null, null, null);

            _stMessage(mBtImage);

            const string message = "Преобразование завершено!";
            const string caption = "Завершение";

            const MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBox.Show(message, caption, buttons);
        }
    }
}
