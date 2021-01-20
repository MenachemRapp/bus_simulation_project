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
    /// Interaction logic for AdjacentStationsWindow.xaml
    /// </summary>
    public partial class AdjacentStationsWindow : Window
    {
        IBL bl;
        int station1, station2;
        public AdjacentStationsWindow(IBL _bl, int _station1, int _station2)
        {
            InitializeComponent();
            bl = _bl;
            station1 = _station1;
            station2 = _station2;

            string title = $"Fill in the values of the drive between:\nstation {station1} and station {station2}.";
            titletb.Text = title;
        }

        private void AddStationDrive_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
