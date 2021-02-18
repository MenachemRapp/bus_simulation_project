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
    /// Interaction logic for Viewing Line Properties Window
    /// </summary>
    public partial class ViewLinePropertiesWindow : Window
    {
        
        public IBL bl;


        BO.LineTotal line;

        public ViewLinePropertiesWindow(IBL _bl, int lineId)
        {
            InitializeComponent();
            bl = _bl;
            line = bl.GetLineNew(lineId);
            RefreshStationList();
            RefreshTripList();
         }


        private void RefreshStationList()
        {
            stationslb.ItemsSource = line.ListOfStation.ToList();
            stationst.DataContext = line;
            
        }
        private void RefreshTripList()
        {
            triplb.ItemsSource = line.ListOfTrips.Where(t => t.Valid).ToList();
            
        }

        

       
    }
}
