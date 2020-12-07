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


namespace targil3B
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window

    {
        ObservableCollection<Bus> buses = new ObservableCollection<Bus>();
        public MainWindow()
        {
            InitializeComponent();
            initBuss();
            //ShowBusLine();
           busList.ItemsSource = buses;
           
            //busList.DisplayMemberPath = "Registration";
            //busList.SelectedIndex = 0;
            //ShowBusLine(0);

        }
        /*private void bus_SelectionChanged(object sender)
        {
            ShowBusLine();
        }*/
        /*private void lbBuses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowBusLine(1);
        }*/

        private void initBuss()
        {

            buses.Add(new Bus("2233322", new DateTime(2000, 11, 11)));
            buses.Add(new Bus("33322333", new DateTime(2020, 11, 11)));
            buses.Add(new Bus("1133311", new DateTime(2007, 01, 01)));
            buses.Add(new Bus("33300333", new DateTime(2019, 03, 28)));

        }

        /*private List<Bus> currentDisplayBus;
        private void ShowBusLine()
        {
            currentDisplayBus = buses;
            DataContext = currentDisplayBus;
            busList.DataContext = currentDisplayBus;

        }*/

        /* private void cmdDeleteUser_Clicked(object sender, RoutedEventArgs e)
         {

             Button cmd = (Button)sender;
             if (cmd.DataContext is Bus)
             {
                 Bus deleteme = (Bus)cmd.DataContext;
                 buses.Remove(deleteme);
                 //bus_SelectionChanged(sender);
             }

         }*/

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

        private void cmdAddBus_Clicked(object sender, RoutedEventArgs e)
        {
            new AddWindow().Show();
           
        }

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
                
                //bus_SelectionChanged(sender);
            }
        }

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

        public void addBus(Bus bus)
        {
           
            if (buses.Contains<Bus>(bus))
            {
                throw new ArgumentException("The bus is already on the list");
               
            }
            buses.Add(bus);                      
        }

       /* private void Buses_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }*/
            }
        }

