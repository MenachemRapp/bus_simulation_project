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
using targil2;

namespace targil3A
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

       
        BusLineData busLines = new BusLineData();
      
        public MainWindow()
        { 
            InitBus();
            InitializeComponent();
            cbBusLines.ItemsSource = busLines;
            cbBusLines.DisplayMemberPath = "BusNumber";
            cbBusLines.SelectedIndex = 0;

        }

        private BusLine currentDisplayBusLine;
        private void cbBusLines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowBusLine((cbBusLines.SelectedValue as BusLine).BusNumber);
        }


        private void ShowBusLine(int index)
        {
            currentDisplayBusLine = busLines[index];
            UpGrid.DataContext = currentDisplayBusLine;
            lbBusLineStations.DataContext = currentDisplayBusLine.Stations;
          
        }


        //Initialize buses with 10 lines
        private void InitBus()
        {
            for (int i = 1; i <= 10; i++)
            {
                BusLine busLine = targil2.RanLine.ranCreateLine(i);
                busLines.AddLineBus(busLine);
            }

        }

        private void lbBusLineStations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }


}
