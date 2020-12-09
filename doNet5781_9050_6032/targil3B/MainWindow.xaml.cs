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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Threading;
using System.Timers;


namespace targil3B
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        ObservableCollection<Bus> buses = new ObservableCollection<Bus>();

        private static System.Timers.Timer aTimer;

        public MainWindow()

        {
                      

        InitializeComponent();
            initBuss();
            busList.DataContext = buses;

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

        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
               showList();
            });
            

        }

        public void showList()
        {
            busList.DataContext=null;
            busList.DataContext = buses;
            
        }

        // initilize bus list
        private void initBuss()
        {

            buses.Add(new Bus("2233322", new DateTime(2000, 11, 11)));
            buses.Add(new Bus("33322333", new DateTime(2020, 11, 11)));
            buses.Add(new Bus("1133311", new DateTime(2007, 01, 01)));
            buses.Add(new Bus("12345841", new DateTime(2019, 03, 28)));
            buses.Add(new Bus("1656486", new DateTime(2000, 11, 11)));
            buses.Add(new Bus("48645467", new DateTime(2020, 11, 11)));
            buses.Add(new Bus("2132183", new DateTime(2007, 01, 01)));
            buses.Add(new Bus("54453487", new DateTime(2019, 03, 28)));
            buses.Add(new Bus("1587538", new DateTime(2000, 11, 11)));
            buses.Add(new Bus("15879630", new DateTime(2020, 11, 11)));
            buses.Add(new Bus("4785369", new DateTime(2007, 01, 01)));
            buses.Add(new Bus("12587961", new DateTime(2019, 03, 28)));

        }

        //click twice on a bus
        private void cmdList_Clicked(object sender, MouseButtonEventArgs e)
        {

            if (e.ClickCount >= 2)
            {
                StackPanel cmd = (StackPanel)sender;
                if (cmd.DataContext is Bus)
                {
                    Bus selectedBus = (Bus)cmd.DataContext;
                    new BusProprtiesWindow(selectedBus).Show();
                   
                }
            }
        }

        //click to add a bus
        private void cmdAddBus_Clicked(object sender, RoutedEventArgs e)
        {
            new AddWindow().Show();
           
        }

        //select a bus to dirve
        private void selectBus_Clicked(object sender, RoutedEventArgs e)
        {
            Button cmd = (Button)sender;
            if (cmd.DataContext is Bus)
            {
                Bus selectedBus = (Bus)cmd.DataContext;
                if (!selectedBus.CanDrive())
                    MessageBox.Show("Bus cannot drive", "Select Bus", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                else
                    new DistanceWindow(selectedBus).Show();
          
            }
        }

        //click to refuel a bus
        private void refuel_Clicked(object sender, RoutedEventArgs e)
        {
            Button cmd = (Button)sender;
            if (cmd.DataContext is Bus)
            {
                Bus selectedBus = (Bus)cmd.DataContext;
                selectedBus.refuel();
                MessageBox.Show("Bus refueled", "Refuel", MessageBoxButton.OK, MessageBoxImage.Information);

            }

        }

        //add a new bus
        public void addBus(Bus bus)
        {
           
            if (buses.Contains<Bus>(bus))
            {
                throw new ArgumentException("The bus is already on the list");
               
            }
            buses.Add(bus);                      
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            showList();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
                    
            this.Close();
           //close all timers

        }
    }
        }

