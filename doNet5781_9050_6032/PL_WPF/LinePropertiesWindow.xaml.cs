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
    /// Interaction logic for LinePropertiesWindow.xaml
    /// </summary>
    public partial class LinePropertiesWindow : Window
    {
        public IBL bl;
        
        public LinePropertiesWindow(IBL _bl, int line)
        {
            InitializeComponent();
            bl = _bl;
            // stationslb.ItemsSource = bl.GetStationCodeNameDistanceTimeInLine(line).ToList();
            stationslb.ItemsSource = bl.GetLineAndStations(line).ListOfStation.ToList();
            stationst.DataContext = bl.GetLineAndStations(line);
        }
    }
}
