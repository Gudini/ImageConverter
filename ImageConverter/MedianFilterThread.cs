﻿using System;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Forms;

namespace ImageConverter
{
    class MedianFilterThread
    {
        public delegate void DelMessage(BitmapImage image);

        public delegate void ProcessBarValue(int value, int procent);

        private readonly DelMessage _delMessage;

        private readonly ProcessBarValue _processBarValue;

        public MedianFilterThread(DelMessage delMessage, ProcessBarValue processBarValue)
        {
            this._delMessage = delMessage;
            this._processBarValue = processBarValue;
        }

        public void StartMedianFilter(int value, BitmapImage mBitmap)
        {
            var bitmapConverter = new BitmapImageToBitmapConverter();
            var mConvertBitmap = (Bitmap)bitmapConverter.ConvertBack(mBitmap, null, null, null);

            int w_b = mConvertBitmap.Width;
            int h_b = mConvertBitmap.Height;
            int k = 1, maxValue = w_b*h_b;
            double curValue=0;

            for (int x = value; x < w_b - value; x++)
            {
                for (int y = value; y < h_b - value; y++)
                {
                    MedianFilter(mConvertBitmap, x, y, value);
                    //curValue = k * 100 / maxValue;
                    //_processBarValue(k, (int) curValue);
                    //k++;
                }
            }

            var bitmapImageConverter = new BitmapImageToBitmapConverter();
            var mBTImage = (BitmapImage)bitmapImageConverter.Convert(mConvertBitmap, null, null, null);

            _delMessage(mBTImage);

            const string message = "Фильтрация завершена!";
            const string caption = "Завершение";

            const MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBox.Show(message, caption, buttons);


        }

        public void MedianFilter(Bitmap myBitmap, int x, int y, int value)
        {
            int n;
            int cR_, cB_, cG_;
            int k = 1;

            n = (2 * value + 1) * (2 * value + 1);

            int[] cR = new int[n + 1];
            int[] cB = new int[n + 1];
            int[] cG = new int[n + 1];

            for (int i = 0; i < n + 1; i++)
            {
                cR[i] = 0;
                cG[i] = 0;
                cB[i] = 0;
            }

            for (int i = x - value; i < x + value + 1; i++)
            {
                for (int j = y - value; j < y + value + 1; j++)
                {
                    var c = myBitmap.GetPixel(i, j);
                    cR[k] = Convert.ToInt32(c.R);
                    cG[k] = Convert.ToInt32(c.G);
                    cB[k] = Convert.ToInt32(c.B);
                    k++;
                }
            }

            Array.Sort(cR);
            Array.Sort(cG);
            Array.Sort(cB);

            int n_ = (int)(n / 2) + 1;

            cR_ = cR[n_];
            cG_ = cG[n_];
            cB_ = cB[n_];

            myBitmap.SetPixel(x, y, Color.FromArgb(cR_, cG_, cB_));

        }

    }
}
