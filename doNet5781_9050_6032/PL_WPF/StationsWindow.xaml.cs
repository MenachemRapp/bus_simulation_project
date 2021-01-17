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
    /// Interaction logic for StationsWindow.xaml
    /// </summary>

    
    public partial class StationsWindow : Window
    {
        IBL bl;
        public StationsWindow(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            StationListlb.ItemsSource = bl.GetAllStationsSorted();
        }

      

        private void SelectStation_clicked(object sender, RoutedEventArgs e)
        {

        }

        private void AddStation_clicked(object sender, RoutedEventArgs e)
        {

        }
    }
}
