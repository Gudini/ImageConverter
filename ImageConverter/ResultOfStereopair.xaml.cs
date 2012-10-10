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
using System.Windows.Shapes;

namespace ImageConverter
{
    /// <summary>
    /// Interaction logic for ResultOfStereopair.xaml
    /// </summary>
    public partial class ResultOfStereopair : Window
    {
        public ResultOfStereopair()
        {
            InitializeComponent();
        }

        public void SetImage(BitmapImage image)
        {
            MainImage.Source = image;
        }
    }
}
