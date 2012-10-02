using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace ImageConverter
{
    class EqualizeThread
    {
        public delegate void EqualizeMessage(BitmapImage image);

        private readonly EqualizeMessage _eqMessage;

        public EqualizeThread(EqualizeMessage eqMessage)
        {
            _eqMessage = eqMessage;
        }

        public void StartEqualize(BitmapImage mBitmap)
        {
            var bitmapConverter = new BitmapImageToBitmapConverter();
            var mConvertBitmap = (Bitmap) bitmapConverter.ConvertBack(mBitmap, null, null, null);

            int w_b = mConvertBitmap.Width;
            int h_b = mConvertBitmap.Height;

            Color minColor = mConvertBitmap.GetPixel(0, 0), maxColor = mConvertBitmap.GetPixel(0, 0);

            int minR = minColor.R, minG = minColor.G, minB = minColor.B;
            int maxR = maxColor.R, maxG = maxColor.G, maxB = maxColor.G;

            for (int i = 0; i < w_b; i++)
            {
                for (int j = 0; j < h_b; j++)
                {
                    Color curColor = mConvertBitmap.GetPixel(i, j);

                    //min value
                    if(curColor.R<minR)
                    {
                        minR = curColor.R;
                    }
                    if (curColor.G < minG)
                    {
                        minG = curColor.G;
                    }
                    if (curColor.B < minB)
                    {
                        minB = curColor.B;
                    }

                    //max value
                    if (curColor.R > maxR)
                    {
                        maxR = curColor.R;
                    }
                    if (curColor.G > maxG)
                    {
                        maxG = curColor.G;
                    }
                    if (curColor.B > maxB)
                    {
                        maxB = curColor.B;
                    }
                }
            }

            float b_R = 255/(maxR - minR);
            float b_G = 255/(maxG - minG);
            float b_B = 255/(maxB - minB);

            float a_R = -1*(b_R*minR);
            float a_G = -1*(b_G*minG);
            float a_B = -1*(b_B*minB);

            for (int i = 0; i < w_b; i++)
            {
                for (int j = 0; j < h_b; j++)
                {
                    Color c = mConvertBitmap.GetPixel(i, j);
                    mConvertBitmap.SetPixel(i, j, Color.FromArgb((int)(a_R + b_R * c.R), (int)(a_G + b_G * c.G), (int)(a_B + b_B * c.B)));
                }
            }

            var bitmapImageConverter = new BitmapImageToBitmapConverter();
            var mBTImage = (BitmapImage)bitmapImageConverter.Convert(mConvertBitmap, null, null, null);

            _eqMessage(mBTImage);

            const string message = "Преобразование завершено!";
            const string caption = "Завершение";

            const MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBox.Show(message, caption, buttons);
        }
    }
}
