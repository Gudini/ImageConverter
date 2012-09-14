using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

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
        }

        private void OpenFileClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = "D:\\";
            dlg.Filter = "Image files (*.jpg)|*.jpg|PNG (*.png)|*.png|BMP (*.bmp)|*.bmp|All Files(*.*)|*.*";
            dlg.RestoreDirectory = true;
            if(dlg.ShowDialog()==true)
            {
                string selectedFileName = dlg.FileName;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(selectedFileName);
                bitmapImage.EndInit();
                ImageView1.Source = bitmapImage;
                this.Height = this.Width/2+25;
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
            int value = Convert.ToInt32(filterDialog.Value);
        }
    }
}
