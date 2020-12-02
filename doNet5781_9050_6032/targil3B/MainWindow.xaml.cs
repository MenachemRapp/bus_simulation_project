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

namespace targil3B
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Bus> buses = new List<Bus>();
        public MainWindow()
        {
            InitializeComponent();
            initBuss();
            busList.ItemsSource = buses;
            //busList.DisplayMemberPath = "Registration";
            //busList.SelectedIndex = 0;
            //ShowBusLine(0);

        }

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

        /*private Bus currentDisplayBus;
        private void ShowBusLine(int index)
        {
            currentDisplayBus = buses[index];
            DataContext = currentDisplayBus;
            busList.DataContext = currentDisplayBus;

        }*/
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
