using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace targil3B
{
    /// <summary>
    /// Interaction logic for BusProprtiesWindow.xaml
    /// </summary>
    public partial class BusProprtiesWindow : Window
    {
        Bus myBus;
        public BusProprtiesWindow(Bus bus)
        {
            InitializeComponent();
            txtRegestration.Content = bus.str_registration();
            txtCirculat.Content = bus.Aliya;
            txtMileage.Content = bus.Kilometer_total;
            txtMaintDate.Content = bus.Maintanence_date;
            txtMaintMile.Content = bus.Kilometer_maintanence;
            txtRefuel.Content = bus.Kilometer_fuel;
            txtStatus.Content = bus.bus_status.ToString();
            myBus = bus;
        }

        private void RefuelClicked(object sender, RoutedEventArgs e)
        {
            this.myBus.refuel();
        }

        private void MaitainClicked(object sender, RoutedEventArgs e)
        {
            this.myBus.maintain();
        }
    }
}
