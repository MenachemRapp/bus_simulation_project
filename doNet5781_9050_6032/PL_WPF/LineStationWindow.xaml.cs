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
    /// Interaction logic for LineStationWindow.xaml
    /// </summary>
    public partial class LineStationWindow : Window
    {

        IBL bl;
        
        public LineStationWindow(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            LinesList.DataContext = bl.GetAllLines().ToList();






        }

        private void UptadeBusLine_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteBusLine_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void PropertiesBusLine_Clicked(object sender, RoutedEventArgs e)
        {

        }
    }
}
