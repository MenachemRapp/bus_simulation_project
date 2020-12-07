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
using System.Timers;

namespace targil3B
{
    /// <summary>
    /// Interaction logic for BusProprtiesWindow.xaml
    /// </summary>
    public partial class BusProprtiesWindow : Window
    {
        Bus myBus;
        private static Timer aTimer;
        public BusProprtiesWindow(Bus bus)
        {

            InitializeComponent();
            txtCirculat.Content = bus.Aliya;
            txtRegestration.Content = bus.str_registration();
            txtMileage.Content = bus.Kilometer_total;
            ShowMaintaineDate(bus);
            ShowMaintaineMileage(bus);
            ShowRefuelMileage(bus);
            ShowStatus(bus);
            ShowStatusTime(bus);

            myBus = bus;

            MaintainClickedEvent += ShowMaintaineDate;
            MaintainClickedEvent += ShowMaintaineMileage;
            MaintainClickedEvent += ShowStatus;
            MaintainClickedEvent += ShowStatusTime;
            RefuelClickedEvent += ShowRefuelMileage;
            RefuelClickedEvent += ShowStatus;
            RefuelClickedEvent += ShowStatusTime;


            // Create a timer and set a one second interval.
            aTimer = new System.Timers.Timer();
            aTimer.Interval = 2000;

            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;

            // Have the timer fire repeated events (true is the default)
            aTimer.AutoReset = true;

            // Start the timer
            aTimer.Enabled = true;


        }

        private  void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                TimeSpan t = TimeSpan.FromSeconds(myBus.TimeStatus);
                txtStatusTime.Content = t.ToString();
            });
            
        }

        //show bus status
        private void ShowStatus(Bus bus)
        {
            txtStatus.Content= bus.bus_status.ToString();
        }

        //show the status timer
        private void ShowStatusTime(Bus bus)
        {
            txtStatusTime.Content = TimeSpan.FromSeconds(bus.TimeStatus).ToString();
        }

        //show the refuel date
        private void ShowRefuelMileage(Bus bus)
        {
            txtRefuel.Content = bus.Kilometer_fuel;
        }

        //show the maintain mileage
        private void ShowMaintaineMileage(Bus bus)
        {
            txtMaintMile.Content = bus.Kilometer_maintanence;
        }
        
        //show the maintain date
        private void ShowMaintaineDate(Bus bus)
        {
            txtMaintDate.Content = bus.Maintanence_date;
        }

        //click refuel
        private void RefuelClicked(object sender, RoutedEventArgs e)
        {
            this.myBus.refuel();
            if (RefuelClickedEvent!=null)
            {
                RefuelClickedEvent(myBus);
            }
                       
        }

        //click maintain
        private void MaitainClicked(object sender, RoutedEventArgs e)
        {
            this.myBus.maintain();
            if (MaintainClickedEvent!=null)
            {
                MaintainClickedEvent(myBus);
            }
            

        }
        public delegate void ButtonClickedHandler(Bus bus);
        public event ButtonClickedHandler MaintainClickedEvent;
        public event ButtonClickedHandler RefuelClickedEvent;

                
    }

}
