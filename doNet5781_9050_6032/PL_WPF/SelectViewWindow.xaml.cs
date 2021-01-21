using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BLAPI;

namespace PL_WPF
{
    /// <summary>
    /// Interaction logic for SelectView.xaml
    /// </summary>
    public partial class SelectViewWindow : Window
    {
        IBL bl;
        public SelectViewWindow(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
        }

        private void LineView_clicked(object sender, RoutedEventArgs e)
        {
            new LineListWindow(bl).Show();
        }

        private void StationView_clicked(object sender, RoutedEventArgs e)
        {
            new StationViewWindow(bl).Show();
        }
    }
}
