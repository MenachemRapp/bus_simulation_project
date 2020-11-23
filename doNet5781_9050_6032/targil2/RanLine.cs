using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace targil2
{
     public class RanLine
    {
        private const double MIN_LAT_AREA = 31;
        private const double LAT_RANGE = 2.3;
        private const double MIN_LON_AREA = 34.3;
        private const double LON_RANGE = 1.2;
        static Random rand = new Random(DateTime.Now.Millisecond);

        //genarate a bus stop
        public static BusStop ranCreateBus()
        {
            int key = rand.Next(1, 1000000);
            foreach (StopAndCounter station in CounterList.StopAndCounterList)
                if (key == station.stop.BusStationKey)
                    return station.stop;

            double latitude = MIN_LAT_AREA + LAT_RANGE * rand.NextDouble();
            double longitude = MIN_LON_AREA + LON_RANGE * rand.NextDouble();

            string address = "";

            return new BusStop(key, latitude, longitude, address);
        }

        //genarate a line bus stop
        public static BusStopLine ranCreateBusStopLine(int index = 1)
        {
            BusStop busStop = ranCreateBus();
            double distance;
            TimeSpan zman;

            if (index > 0)
            {
                distance = rand.Next(20) + rand.NextDouble();
                zman = TimeSpan.FromMinutes(rand.Next(60));
            }
            else
            {
                distance = 0;
                zman = TimeSpan.Zero;
            }
            return new BusStopLine(busStop, distance, zman);
        }

        //genarate a line
        public static BusLine ranCreateLine(int lineNum)
        {

            BusLine newLine = new BusLine();
            newLine.BusNumber = lineNum;

            BusStopLine newStop;
            for (int i = 0; i < 8; i++)
            {
                do
                {
                    newStop = ranCreateBusStopLine(i);
                } while (newLine.findStop(newStop.Stop));
                newLine.add(newStop, i);
            }

            Area area;
            Enum.TryParse(Convert.ToString(rand.Next(5)), out area);
            newLine.Area = area;


            return newLine;
        }
    }
}
