﻿using System;
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
    /// Interaction logic for editing station window
    /// </summary>
    public partial class StationPropertiesWindow : Window
    {
        IBL bl;
        int stationId;
        public StationPropertiesWindow(IBL _bl, int _stationId)
        {
            InitializeComponent();
            bl = _bl;
            stationId = _stationId;
            BO.StationWithLines station = bl.GetStationWithLines(stationId);
            StationListlb.ItemsSource= station.ListOfLines.ToList();
            stationsp1.DataContext= station;
            stationsp2.DataContext = station;
            listTitle.DataContext = station.ListOfLines.ToList().Count();
            DeleteButton.DataContext = station.ListOfLines.ToList().Count();
        }

        private void DeleteButton_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.DeleteStation(stationId);
                if (DeleteEvent != null)
                    DeleteEvent(sender,e);
                this.Close();
            }
            catch (BO.BadStationCodeException ex)
            {
                MessageBox.Show(ex.Message, "Delete Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public EventHandler DeleteEvent;
    }
}
