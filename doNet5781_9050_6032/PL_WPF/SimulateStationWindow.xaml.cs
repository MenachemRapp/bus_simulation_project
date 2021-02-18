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
        IEnumerable<BO.TripAndStations> fullTimingList;
        BO.LineTiming lineTiming;
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
            tripTitle.DataContext = fullTimingList.ToList().Count();
            refreshTimeList();

            StationListlb.ItemsSource = station.ListOfLines.ToList();


            this.Closed+=(x,y)=> { bl.SetStationPanel(-1, timing => {driverWorker.ReportProgress(55); }); driverWorker.CancelAsync(); };

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
                MessageBox.Show("Simulation is off.\n Showing all trips.", "Notificaiton", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            

        }

        private void refreshTimeList()
        {
           
            TimeSpan timeNow = bl.GetTime();
            timingList = bl.GetLineTimingsFromFullList(stationId, fullTimingList);
            StationTimeListlb.ItemsSource = bl.GetFirstTimingForEachLine(timingList);

            BO.LineTiming lastTiming = bl.LastLineTiming(timingList);
            if (lastTiming!=null)
            {
                LastBusSp.DataContext = lastTiming;
            }
            else //no trips in this line
            {
                LastBusSp.Visibility = Visibility.Collapsed;
                LastBusTitle.Visibility = Visibility.Collapsed;
            }                    
            
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
           bl.SetStationPanel(stationId, timing=> { lineTiming=timing; driverWorker.ReportProgress(55); });
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            fullTimingList=bl.UpdateNewTimingInList(fullTimingList,lineTiming);
            refreshTimeList();
        }



    }
}
