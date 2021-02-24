using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
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
    /// Interaction logic for Simulate Station Window
    /// </summary>
    public partial class SimulateStationWindow : Window
    {
        IBL bl;
        int stationId;
        IEnumerable<BO.LineTiming> timingList;
        IEnumerable<BO.LineTiming> nowTimingList;
        IEnumerable<BO.TripAndStations> fullTimingList, unsetFullTimingList;
        BackgroundWorker driverWorker;
    

        public SimulateStationWindow(IBL _bl, int _stationId)
        {
            InitializeComponent();
            bl = _bl;
            stationId = _stationId;

            BO.StationWithLines station = bl.GetStationWithLines(stationId);
            unsetFullTimingList = bl.GetTripListByStation(stationId).ToList();


            stationsp1.DataContext = station;
            stationsp2.DataContext = station;
            listTitle.DataContext = station.ListOfLines.ToList().Count();
            tripTitle.DataContext = unsetFullTimingList.ToList().Count();
            initializeList();
            nowTimingList = timingList;
            refreshTimeList();

            StationListlb.ItemsSource = station.ListOfLines.ToList();


            this.Closed += (x, y) =>
            {
                bl.SetStationPanel(-1, driverAction);
                bl.RemoveFromObserver(timerAction);
                driverWorker.CancelAsync();
            };

            driverWorker = new BackgroundWorker();
            driverWorker.DoWork += driverWorker_DoWork;
            driverWorker.ProgressChanged += driverWorker_ProgressChanged;
            driverWorker.WorkerReportsProgress = true;
            driverWorker.WorkerSupportsCancellation = true;
           
            try
            {
                bl.GetRate();//tests is the timer is on
                driverWorker.RunWorkerAsync();
                bl.AddToObserver(timerAction);
                
            }
            catch (Exception)
            {
                MessageBox.Show("Simulation is off.\n Showing all trips.", "Notificaiton", MessageBoxButton.OK, MessageBoxImage.Information);
            }


        }

        /// <summary>
        /// initialize timing from the full list
        /// </summary>
        private void initializeList()
        {
            fullTimingList = bl.InitTripListFromNow(unsetFullTimingList);
            timingList = bl.GetLineTimingsFromFullList(stationId, fullTimingList);
        }

        /// <summary>
        /// refresh if bus has arrived
        /// </summary>
        private void refreshBusAtStop()
        {
            timingList = bl.GetLineTimingsFromFullList(stationId, fullTimingList);
        }

        /// <summary>
        /// set timing lists
        /// </summary>
        private void refreshTimeList()
        {

            StationTimeListlb.ItemsSource = bl.GetFirstTimingForEachLine(nowTimingList);

            BO.LineTiming lastTiming = bl.LastLineTiming(timingList);
            if (lastTiming != null)
            {
                LastBusSp.DataContext = lastTiming;
            }
            else //no trips in this line
            {
                LastBusSp.Visibility = Visibility.Collapsed;
                LastBusTitle.Visibility = Visibility.Collapsed;
            }

        }


        private void driverAction(BO.LineTiming newTiming)
        {
            driverWorker.ReportProgress(33, newTiming);
        }

        private void driverWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            bl.SetStationPanel(stationId, driverAction);
        }

        private void driverWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (((BO.LineTiming)e.UserState).TripId == -1)// restart the timer for a new day
            {
                initializeList();
                refreshBusAtStop();
            }
            else
            {
                fullTimingList = bl.UpdateNewTimingInList(fullTimingList, (BO.LineTiming)e.UserState);
                refreshBusAtStop();
            }
        }
     

        private void timerAction(TimeSpan time)
        {
            nowTimingList = bl.UpdateTimeNow(timingList, time);
            Dispatcher.Invoke(refreshTimeList); //didn't use background worker becuase it happens every seccond            
        }

     
    }
}
