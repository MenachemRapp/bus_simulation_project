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
            txtCirculat.Content = bus.Aliya;
            txtRegestration.Content = bus.str_registration();
            txtMileage.Content = bus.Kilometer_total;
           
            ShowMaintaineDate(bus.Maintanence_date);
            ShowMaintaineMilage(bus.Kilometer_maintanence);
            ShowRefuelMilage(bus.Kilometer_fuel);
            
            ShowStatus(bus.bus_status);
            
            myBus = bus;
        }

        private void ShowStatus(BUS_STATUS bus_status)
        {
            txtStatus.Content=bus_status.ToString();
        }
                

        private void ShowRefuelMilage(int kilometer_fuel)
        {
            txtRefuel.Content = kilometer_fuel;
        }

        private void ShowMaintaineMilage(int kilometer_maintanence)
        {
            txtMaintMile.Content = kilometer_maintanence;
        }

        private void RefuelClicked(object sender, RoutedEventArgs e)
        {
            this.myBus.refuel();
            ShowRefuelMilage(myBus.Kilometer_fuel);
            ShowStatus(myBus.bus_status);
           
        }

        private void MaitainClicked(object sender, RoutedEventArgs e)
        {
            this.myBus.maintain();
            ShowMaintaineDate(myBus.Maintanence_date);
            ShowMaintaineMilage(myBus.Kilometer_maintanence);
            ShowStatus(myBus.bus_status);
          
        }
    
        private void ShowMaintaineDate(DateTime date)
        {
            txtMaintDate.Content = date;
        }
    }
}
