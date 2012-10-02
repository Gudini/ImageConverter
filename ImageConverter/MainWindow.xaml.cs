using System;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Drawing;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace ImageConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static BitmapImage _mBtImage;
        private readonly MedianFilterThread _mReCal;
        private int maxValueProcessBar;

        private readonly NegativeThread _mNegCal;

        private readonly EqualizeThread _mEqCal;

        public MainWindow()
        {
            InitializeComponent();

            _mReCal = new MedianFilterThread(delMessage, processBarValue);
            _mNegCal = new NegativeThread(NegDelMessage, NegProcessBarValue);
            _mEqCal = new EqualizeThread(EqualizeMessage);
        }

        public void delMessage(BitmapImage image)
        {
            Action action = new Action(delegate
                                           {
                                               ImageView2.Source = image;
                                               MedianFilterItem.IsEnabled = true;
                                               NegativItem.IsEnabled = true;
                                               EqualizeItem.IsEnabled = true;
                                               filterProcessBar.Value = 0;
                                               LabelValueProcent.Content = "0 %";
                                               //ImageView2.IsReadOnly = false;
                                               OpenItem.IsEnabled = true;
                                           });
            ImageView2.Dispatcher.Invoke(action, DispatcherPriority.Normal);
        }

        public void processBarValue(int value, int procent)
        {
            Action action = new Action(delegate
                                           {
                                               filterProcessBar.Value = value;
                                               LabelValueProcent.Content = procent+" %";
                                           });
            filterProcessBar.Dispatcher.Invoke(action, DispatcherPriority.Normal);
        }

        public void NegDelMessage(BitmapImage image)
        {
            Action action = new Action(delegate
                                           {
                                               ImageView2.Source = image;
                                               MedianFilterItem.IsEnabled = true;
                                               NegativItem.IsEnabled = true;
                                               EqualizeItem.IsEnabled = true;
                                               filterProcessBar.Value = 0;
                                               LabelValueProcent.Content = "0 %";
                                               //ImageView2.IsReadOnly = false;
                                               OpenItem.IsEnabled = true;
                                           });
            ImageView2.Dispatcher.Invoke(action, DispatcherPriority.Normal);
        }

        public void EqualizeMessage(BitmapImage image)
        {
            Action action = new Action(delegate
                                           {
                                               ImageView2.Source = image;
                                               MedianFilterItem.IsEnabled = true;
                                               NegativItem.IsEnabled = true;
                                               EqualizeItem.IsEnabled = true;
                                               OpenItem.IsEnabled = true;
                                           });
            ImageView2.Dispatcher.Invoke(action, DispatcherPriority.Normal);
        }

        public void NegProcessBarValue(int value, int procent)
        {
            Action action = new Action(delegate
                                           {
                                               filterProcessBar.Value = value;
                                               LabelValueProcent.Content = procent + " %";
                                           });
            filterProcessBar.Dispatcher.Invoke(action, DispatcherPriority.Normal);
        }

        private void OpenFileClick(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
                          {
                              InitialDirectory = "D:\\Images",
                              Filter = "Image files (*.jpg)|*.jpg|PNG (*.png)|*.png|BMP (*.bmp)|*.bmp|All Files(*.*)|*.*",
                              RestoreDirectory = true
                          };
            if(dlg.ShowDialog()==true)
            {
                NegativItem.IsEnabled = true;
                MedianFilterItem.IsEnabled = true;
                EqualizeItem.IsEnabled = true;
                
                string selectedFileName = dlg.FileName;
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(selectedFileName);
                bitmapImage.EndInit();
                ImageView1.Source = bitmapImage;

                ImageView2.IsEnabled = true;
                //ImageView2.Source = bitmapImage;
                _mBtImage = bitmapImage;

            }
        }

        private void ExitFileClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MedianFilterClick(object sender, RoutedEventArgs e)
        {
            int value = 1;

            var bitmapConverter = new BitmapImageToBitmapConverter();
            Bitmap mConvertBitmap = (Bitmap)bitmapConverter.ConvertBack(_mBtImage, null, null, null);

            int w_b = mConvertBitmap.Width;
            int h_b = mConvertBitmap.Height;

            filterProcessBar.Maximum = w_b*h_b;
            maxValueProcessBar = w_b*h_b;

            var thread = new Thread(() => _mReCal.StartMedianFilter(value, _mBtImage));
            thread.Start();

            MedianFilterItem.IsEnabled = false;
            NegativItem.IsEnabled = false;
            EqualizeItem.IsEnabled = false;
            OpenItem.IsEnabled = false;
            //ImageView1.IsReadOnly = true;
            //ImageView2.IsReadOnly = true;

        }

        private void NegetiveClick(object sender, RoutedEventArgs e)
        {
            var bitmapConverter = new BitmapImageToBitmapConverter();
            Bitmap mConvertBitmap = (Bitmap)bitmapConverter.ConvertBack(_mBtImage, null, null, null);

            int w_b = mConvertBitmap.Width;
            int h_b = mConvertBitmap.Height;

            filterProcessBar.Maximum = w_b * h_b;
            maxValueProcessBar = w_b * h_b;

            var thread = new Thread(() => _mNegCal.StartNegative(_mBtImage));
            thread.Start();

            MedianFilterItem.IsEnabled = false;
            NegativItem.IsEnabled = false;
            EqualizeItem.IsEnabled = false;

            OpenItem.IsEnabled = false;
            //ImageView1.IsReadOnly = true;
            //ImageView2.IsReadOnly = true;
        }

        private void EqualizeClick(object sender, RoutedEventArgs e)
        {
            var bitmapConverter = new BitmapImageToBitmapConverter();
            Bitmap mConverterBitmap = (Bitmap) bitmapConverter.ConvertBack(_mBtImage, null, null, null);

            int w_b = mConverterBitmap.Width;
            int h_b = mConverterBitmap.Height;

            var thread = new Thread(() => _mEqCal.StartEqualize(_mBtImage));
            thread.Start();

            MedianFilterItem.IsEnabled = false;
            NegativItem.IsEnabled = false;
            EqualizeItem.IsEnabled = false;

            OpenItem.IsEnabled = false;

            /*
            int width = mConverterBitmap.Width;
            int height = mConverterBitmap.Height;

            Color minColor = mConverterBitmap.GetPixel(0, 0), maxColor = mConverterBitmap.GetPixel(0, 0);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Color curColor = mConverterBitmap.GetPixel(i, j);
                    if (minColor.GetBrightness() > curColor.GetBrightness())
                    {
                        minColor = curColor;
                    }

                    if (maxColor.GetBrightness() < curColor.GetBrightness())
                    {
                        maxColor = curColor;
                    }
                }
            }

            float brightness = -0.76F;

            float[][] colorMatrixElements = {
                                                new float[] {brightness, 0, 0, 0, 0},
                                                new float[] {0, brightness, 0, 0, 0},
                                                new float[] {0, 0, brightness, 0, 0},
                                                new float[] {0, 0, 0, brightness, 0},
                                                new float[] {brightness, brightness, brightness, 0, 1}
                                            };

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            ImageAttributes imageAttributes = new ImageAttributes();
            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            Graphics graphics = Graphics.FromImage(mConverterBitmap);
            graphics.DrawImage(mConverterBitmap, new Rectangle(0, 0, width, height), 0, 0, width, height, GraphicsUnit.Pixel, imageAttributes);

            ImageView2.Source = (BitmapImage) bitmapConverter.Convert(mConverterBitmap, null, null, null);
             */
        }
    }
}
