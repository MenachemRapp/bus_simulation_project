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
            if (timingList.Where(t => t.TimeAtStop < timeNow).Count()>0)
            {
                LastBusSp.DataContext = timingList.
                Where(t => t.TimeAtStop < timeNow).
                OrderByDescending(t => t.TimeAtStop).
                First();
                
            }
            else if (timingList.Count() > 0)
            {
                LastBusSp.DataContext = timingList.
                OrderByDescending(t => t.TimeAtStop).
                First();
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
