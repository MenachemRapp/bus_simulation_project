﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;
using BLAPI;

namespace PL_WPF
{
    /// <summary>
    /// Interaction logic for Simulation Main Window
    /// </summary>
    public partial class SimulationAndViewWindow : Window
    {

        bool isTimerRun;
        BackgroundWorker timerworker;

        string textTime;

        TimeSpan setTime;
        int ratio;

        IBL bl;
        public SimulationAndViewWindow(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            this.Closed += CloseChildren;
            this.Closed += (x, y) =>
            {
                if (isTimerRun) bl.StopSimulator();
             //   if (timerworker.IsBusy) timerworker.CancelAsync();
            };


            isTimerRun = false;
            timerworker = new BackgroundWorker();
            timerworker.DoWork += Worker_DoWork;
            timerworker.ProgressChanged += Worker_ProgressChanged;
            timerworker.WorkerReportsProgress = true;
            timerworker.WorkerSupportsCancellation = true;
        }

        /// <summary>
        /// closes chlidren windows. used when window is closed
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void CloseChildren(object sender, EventArgs e)
        {
            ViewLineListWindow lineWindow = Application.Current.Windows.OfType<ViewLineListWindow>().FirstOrDefault();
            StationsSimulateListWindow stationWindow = Application.Current.Windows.OfType<StationsSimulateListWindow>().FirstOrDefault();
            if (lineWindow != null)
                lineWindow.Close();
            if (stationWindow != null)
                stationWindow.Close();
        }


        private void LineView_clicked(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            button.IsEnabled = false;
            ViewLineListWindow lineWin = new ViewLineListWindow(bl);
            lineWin.Show();
            lineWin.Closed += (x, y) => button.IsEnabled = true;
        }

        private void StationView_clicked(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            button.IsEnabled = false;
            StationsSimulateListWindow stationWin = new StationsSimulateListWindow(bl);
            stationWin.Show();
            stationWin.Closed += (x, y) => button.IsEnabled = true;
        }

        //start and stop simulation
        private void Timer_clicked(object sender, RoutedEventArgs e)
        {
            if (!isTimerRun && !timerworker.IsBusy)
            {
                setTime = TimeSpan.FromHours(Double.Parse(hoursTb.Text)) + TimeSpan.FromMinutes(Double.Parse(minutesTb.Text)) + TimeSpan.FromSeconds(Double.Parse(secondsTb.Text));

                isTimerRun = true;

                ratio = int.Parse(ratioTb.Text);

                timerworker.RunWorkerAsync();
                timerButton.Content = "Stop simulation";
                timerButton.Background = Brushes.Red;
                enterTimeSp.Visibility = Visibility.Collapsed;
                ratioSp.Visibility = Visibility.Collapsed;
                timerTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                isTimerRun = false;
                bl.StopSimulator();

                timerButton.Content = "Start simulation";
                timerButton.Background = SystemColors.ControlBrush;
                enterTimeSp.Visibility = Visibility.Visible;
                timerTextBlock.Visibility = Visibility.Collapsed;
                ratioSp.Visibility = Visibility.Visible;
            }
        }



        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            bl.StartSimulator(setTime, ratio, time => { textTime = time.ToString().Substring(0, 8); timerworker.ReportProgress(55); });

        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            timerTextBlock.Text = textTime.ToString().Substring(0, 8);

        }

        //change to editing mode
        private void EditingMode_Click(object sender, RoutedEventArgs e)
        {
            new SelectEditWindow(bl).Show();
            this.Close();
        }
    }

}
