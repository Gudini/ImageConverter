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
    /// Interaction logic for FilterDialog.xaml
    /// </summary>
    public partial class FilterDialog : Window
    {
        public FilterDialog()
        {
            InitializeComponent();
        }

        private String value;

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            pixelTextBox.Text = String.Format("{0:0}", slider.Value);
            if(okbutton!=null)
            {
                okbutton.IsEnabled = true;
            }
        }

        public  String Value
        {
            get { return value; }
        }

        private void OkClick(object sender, RoutedEventArgs e)
        {
            value = String.Format("{0:0}", slider.Value);
            DialogResult = true;
            this.Close();
        }

        private void pixelTextChanged(object sender, TextChangedEventArgs e)
        {
            if(!pixelTextBox.Text.Equals(""))
            {
                slider.Value = Convert.ToInt32(pixelTextBox.Text);
                if (okbutton != null)
                {
                    okbutton.IsEnabled = true;
                }
            }
            else
            {
                okbutton.IsEnabled = false;
            }
        }
    }
}
