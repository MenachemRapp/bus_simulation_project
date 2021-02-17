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
    /// Interaction logic for SimulateStationWindow.xaml
    /// </summary>
    public partial class SimulateStationWindow : Window
    {
        IBL bl;
        int stationId;
        IEnumerable<BO.LineTiming> timingList;
        IEnumerable<BO.TripAndStations> fullTimingList;

        BackgroundWorker driverWorker;

        public SimulateStationWindow(IBL _bl, int _stationId)
        {
            InitializeComponent();
            bl = _bl;
            stationId = _stationId;
            
            BO.StationWithLines station = bl.GetStationWithLines(stationId);
            fullTimingList = bl.GetTripListByStation(stationId);
            
            
            stationsp1.DataContext = station;
            stationsp2.DataContext = station;
            listTitle.DataContext = station.ListOfLines.ToList().Count();
            refreshTimeList();

            StationListlb.ItemsSource = timingList.GroupBy(t => t.LineId).Select(group => group.First());
           
            this.Closed+=(x,y)=> { bl.SetStationPanel(-1, timing => { update_timing(timing); driverWorker.ReportProgress(55); }); driverWorker.CancelAsync(); };

            driverWorker = new BackgroundWorker();
            driverWorker.DoWork += Worker_DoWork;
            driverWorker.ProgressChanged += Worker_ProgressChanged;
            driverWorker.WorkerReportsProgress = true;
            driverWorker.WorkerSupportsCancellation = true;
            try
            {
                bl.GetRate();
                driverWorker.RunWorkerAsync();
            }
            catch (Exception)
            {
                MessageBox.Show("Simulation is off.\n Showing all stations.", "Notificaiton", MessageBoxButton.OK, MessageBoxImage.Information);
               
            }
            

        }

        private void refreshTimeList()
        {
            TimeSpan timeNow = bl.GetTime();
            timingList = bl.GetLineTimingsFromFullList(stationId, fullTimingList);
            StationTimeListlb.ItemsSource = timingList.
                Where(t => t.TimeAtStop > timeNow).
                GroupBy(t => t.LineId).
                Select(group => group.FirstOrDefault(t => t.TimeAtStop == group.Min(tr => tr.TimeAtStop))).
                OrderBy(t => t.StartTime);

        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
           bl.SetStationPanel(stationId, timing=> { update_timing(timing); driverWorker.ReportProgress(55); });
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            
            refreshTimeList();
        }


        private void update_timing(BO.LineTiming newTiming)
        {
            BO.TripAndStations tripList= fullTimingList.FirstOrDefault(trip => trip.LineId == newTiming.LineId && trip.startTime == newTiming.StartTime);
            BO.StationTime stationTime = tripList.ListOfStationTime.FirstOrDefault(st => st.station == newTiming.Code);
            tripList.ListOfStationTime =tripList.ListOfStationTime.Select(t =>
            {
                if (t.index < stationTime.index)
                {
                    return t;
                }
                else if (t.index == stationTime.index)
                    return new BO.StationTime { index = t.index, station = t.station, timeAtStop = newTiming.TimeAtStop, timeToNextStop = t.timeToNextStop };
                else
                    return new BO.StationTime { index = t.index, station = t.station, timeAtStop = t.timeAtStop + newTiming.TimeAtStop - stationTime.timeAtStop, timeToNextStop = t.timeToNextStop };
            });

            fullTimingList = fullTimingList.Where(trip => trip.LineId != newTiming.LineId || trip.startTime != newTiming.StartTime).Append(tripList).OrderBy(t => t.startTime);
           


        }

    }
}
