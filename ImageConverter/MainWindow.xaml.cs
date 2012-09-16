using System;
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
        public MainWindow()
        {
            InitializeComponent();

            _mReCal = new MedianFilterThread(delMessage, processBarValue);
        }

        private static BitmapImage _mBtImage;
        private readonly MedianFilterThread _mReCal;
        private int maxValueProcessBar;

        public void delMessage(BitmapImage image)
        {
            Action action = new Action(delegate
                                           {
                                               ImageView2.Source = image;
                                               MedianFilterItem.IsEnabled = true;
                                               NegativItem.IsEnabled = true;
                                               filterProcessBar.Value = 0;
                                               LabelValueProcent.Content = "0 %";
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

        private void OpenFileClick(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
                          {
                              InitialDirectory = "D:\\",
                              Filter =
                                  "Image files (*.jpg)|*.jpg|PNG (*.png)|*.png|BMP (*.bmp)|*.bmp|All Files(*.*)|*.*",
                              RestoreDirectory = true
                          };
            if(dlg.ShowDialog()==true)
            {
                NegativItem.IsEnabled = true;
                MedianFilterItem.IsEnabled = true;
                
                string selectedFileName = dlg.FileName;
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(selectedFileName);
                bitmapImage.EndInit();
                ImageView1.Source = bitmapImage;

                ImageView2.IsEnabled = true;
                ImageView2.Source = bitmapImage;
                _mBtImage = bitmapImage;

            }
        }

        private void ExitFileClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MedianFilterClick(object sender, RoutedEventArgs e)
        {
            var filterDialog = new FilterDialog();
            filterDialog.ShowDialog();

            if(filterDialog.DialogResult==true)
            {
                int value = Convert.ToInt32(filterDialog.Value);

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
            }
        }

    }
}
