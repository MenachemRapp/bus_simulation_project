using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace targil2
{
    class Program
    {
        private const double MIN_LAT_AREA = 31;
        private const double LAT_RANGE = 2.3;
        private const double MIN_LON_AREA = 34.3;
        private const double LON_RANGE = 1.2;
        static Random rand = new Random(DateTime.Now.Millisecond);
        static void Main(string[] args)
        {
            BusLineData buses=new BusLineData();
            BusStop busStop;
            BusStopLine busStopLine;
            BusLine busLine;

            int lineNum;
            double distance;
            TimeSpan zman;

            //creates at lest 10 lines with at least 40 stops all together
            for (int i = 1; i <= 10||CounterList.StopAndCounterList.Count<40; i++)
            {
                busLine = ranCreateLine(i);
                buses.AddLineBus(busLine);
            }

            //adds 10 stops that have at least 2 lines going through them
            for (int i = 0; i < 10; i++)
            {
                int index = rand.Next(1, 9);
                busStopLine = ranCreateBusStopLine(index);
                int mix = rand.Next(1, 10);
                if(!buses[mix].findStop(busStopLine.Stop))
                buses[mix].add(busStopLine, index, rand.Next(20) + rand.NextDouble(), TimeSpan.FromMinutes(rand.Next(60)));
                if (!buses[11-mix].findStop(busStopLine.Stop))
                    buses[11-mix].add(busStopLine, index, rand.Next(20) + rand.NextDouble(), TimeSpan.FromMinutes(rand.Next(60)));

            }
      
            CHOICE choice; ;
            
            bool success = true;
            do
            {
                Console.WriteLine("Please, make your choice:");
                Console.WriteLine("ADD, REMOVE, FIND, PRINT, EXIT");
                success = Enum.TryParse(Console.ReadLine(), out choice);
                if (!success)
                {
                    continue;
                }

                switch (choice)
                {
                    case CHOICE.ADD:
                        ADD add;
                        Console.WriteLine("Please, make your choice:");
                        Console.WriteLine("ADD_LINE,ADD_BUS_STOP");
                        success = Enum.TryParse(Console.ReadLine(), out add);
                        if (!success)
                        {
                            continue;
                        }

                        switch (add)
                        {
                            case ADD.ADD_LINE:
                                busLine = createLine();
                                buses.AddLineBus(busLine);

                                break;
                            case ADD.ADD_BUS_STOP:
                                lineNum = createLineNum();
                                Console.WriteLine("which number is the stop (starting with 1)?");
                                int index = Convert.ToInt32(Console.ReadLine()) - 1;
                                busStopLine = createBusStopLine(index);
                                distance=createNextDistance(index != buses[lineNum].Stations.Count);
                                zman = createNextZman(index != buses[lineNum].Stations.Count);
                                buses[lineNum].add(busStopLine,index,distance,zman);
                                break;

                            default:

                                break;
                        }
                        break;
                    case CHOICE.REMOVE:
                        REMOVE remove;
                        Console.WriteLine("Please, make your choice:");
                        Console.WriteLine("REMOVE_LINE,REMOVE_BUS_STOP");
                        success = Enum.TryParse(Console.ReadLine(), out remove);
                        if (!success)
                        {
                            continue;
                        }
                        switch (remove)
                        {
                            case REMOVE.REMOVE_LINE:
                                lineNum = createLineNum();
                                int firstId = createFirstId(buses,lineNum);
                                buses.deleteLine(lineNum,firstId);
                                break;
                            case REMOVE.REMOVE_BUS_STOP:
                                lineNum = createLineNum();
                                busStop = createBus();
                                int index = buses[lineNum].Stations.FindIndex(x => x.Stop.BusStationKey == busStop.BusStationKey);
                                if (index == -1)
                                    throw new ArgumentException("the station isn't in the line");
                                distance = createNextDistance(index != 0 && index != buses[lineNum].Stations.Count-1);
                                zman = createNextZman(index != 0 && index != buses[lineNum].Stations.Count-1);
                               buses[lineNum].remove(busStop,distance,zman);
                                break;
                            default:
                                break;
                        }
                        break;
                    
                    case CHOICE.FIND:
                        FIND find;
                        Console.WriteLine("FIND_LINES_IN_STOP, OPTIONS_BETWEEN_STOPS");
                        success = Enum.TryParse(Console.ReadLine(), out find);
                        switch (find)
                        {
                            case FIND.FIND_LINES_IN_STOP:
                                Console.WriteLine("Please enter the station number");
                                buses.printLineAtStop(Convert.ToInt32(Console.ReadLine()));
                                break;
                            case FIND.OPTIONS_BETWEEN_STOPS:
                                int id_first, id_last;
                                Console.WriteLine("please enter the first stop id, and last stop id");
                                id_first = Convert.ToInt32(Console.ReadLine());
                                id_last= Convert.ToInt32(Console.ReadLine());
                                BusLineData temp_buses = new BusLineData();
                                foreach (BusLine bus in buses)
                                {
                                    BusLine temp_line = bus.subroute(id_first, id_last);
                                    if (temp_line != null)
                                        temp_buses.AddLineBus(temp_line);
                                }
                                Console.WriteLine(temp_buses.sortedList());  
                                break;
                            default:
                                break;
                        }
                        break;
                    case CHOICE.PRINT:
                        PRINT print;
                        Console.WriteLine("PRINT_LINES, PRINT_STOPS");
                        success = Enum.TryParse(Console.ReadLine(), out print);
                        switch (print)
                        {
                            case PRINT.PRINT_LINES:
                                Console.WriteLine(buses);
                                break;
                            case PRINT.PRINT_STOPS:
                                foreach (StopAndCounter stop in CounterList.StopAndCounterList)
                                {
                                    buses.printLineAtStop(stop.stop.BusStationKey);//error stop 2 times!!!!
                                    Console.WriteLine("");
                                }
                                
                                break;
                            default:
                                break;
                        }
                        break;
                    case CHOICE.EXIT:
                        break;
                    default:
                        break;
                }
            }
            while (choice != CHOICE.EXIT);
        }

        //receives a bus stop
        private static BusStop createBus()
        {
            Console.WriteLine("enter bus station key:");
            int key = Convert.ToInt32(Console.ReadLine());
            foreach (StopAndCounter station in CounterList.StopAndCounterList)
                if (key == station.stop.BusStationKey)
                        return station.stop;

            double latitude = MIN_LAT_AREA + LAT_RANGE * rand.NextDouble();
            double longitude = MIN_LON_AREA + LON_RANGE * rand.NextDouble();

            Console.WriteLine("enter address");
            string address = Console.ReadLine();

            return new BusStop(key, latitude, longitude, address);
        }

        //receives the distance of the next stop
        private static double createNextDistance(bool test)
        {
            double distance = 0;
            if(test)
            {
                Console.WriteLine("New distance of the next station:");
                distance = Convert.ToDouble(Console.ReadLine());
            }

            return distance;
        }

        //receives the time of the next stop
        private static TimeSpan createNextZman(bool test)
        {
            TimeSpan zman = TimeSpan.Zero;
            if (test)
            {
                Console.WriteLine("New time (by minutes) of next station:");
                zman = TimeSpan.FromMinutes(Convert.ToDouble(Console.ReadLine()));
            }

            return zman;
        }

        //receives a line bus stop
        private static BusStopLine createBusStopLine(int index)
        {
            BusStop busStop = createBus();
            double distance;
            TimeSpan zman;
                       
            if (index>0)
            {
                Console.WriteLine("what is the distance from the last stop?");
                distance = Convert.ToDouble(Console.ReadLine());
                Console.WriteLine("how many minutes past since the last stop?");
                zman = TimeSpan.FromMinutes(Convert.ToDouble(Console.ReadLine()));
            }
            else
            {
                distance = 0;
                zman = TimeSpan.Zero;
            }
            return new BusStopLine(busStop, distance, zman);
        }
        
        //receives a line number
        private static int createLineNum()
        {
            Console.WriteLine("choose line number:");
            int line = Convert.ToInt32(Console.ReadLine());
            return line;
        }

        //finds first station id
        private static int createFirstId(BusLineData buses, int lineNum)
        {
            int firstStop=new int();
            switch (buses.busesInLine(lineNum))
            {
                case 2:
                    Console.WriteLine("is the first stop {0} or {1} ?", buses[lineNum].FirstStation.Stop.BusStationKey, buses[lineNum].LastStation.Stop.BusStationKey);
                    firstStop = Convert.ToInt32(Console.ReadLine());
                    if (firstStop != buses[lineNum].FirstStation.Stop.BusStationKey && firstStop != buses[lineNum].LastStation.Stop.BusStationKey)
                        throw new ArgumentException(String.Format("{0} was not one of the options", Convert.ToString(firstStop)));
                    break;
                case 1:
                    firstStop = buses[lineNum].FirstStation.Stop.BusStationKey;
                    break;
                case 0:
                    throw new ArgumentException(String.Format("line number {0} does not exist", Convert.ToString(lineNum)));
                default:
                    break;
            }
                     
            return firstStop;
        }

        //receives a line
        private static BusLine createLine()
        {
            BusLine newLine = new BusLine();
            newLine.BusNumber = createLineNum();
            
            Console.WriteLine("how many stops does the bus line have?");
            int length = Convert.ToInt32(Console.ReadLine());
            if (length < 2)
                throw new ArgumentOutOfRangeException("the line must have at least 2 stations");
            

            for (int i = 0; i < length; i++)
            {
                Console.WriteLine("stop number {0}",i+1);
                BusStopLine newStop = createBusStopLine(i);
                if(newLine.findStop(newStop.Stop))
                    throw new ArgumentException(String.Format("stop number {0} already exists in the line", (newStop.Stop.BusStationKey)));
                newLine.add(newStop, i);
            }

            Console.WriteLine("Chose an area for the bus line");
            Console.WriteLine("General,Jerusalem,North,South,Center");

           Area area;
           Enum.TryParse(Console.ReadLine(), out area);
            newLine.Area = area;
           return newLine;
        }

        //the same functions as before, but this time the computer genarates the answer

        //genarate a bus stop
        private static BusStop ranCreateBus()
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
        private static BusStopLine ranCreateBusStopLine(int index=1)
        {
            BusStop busStop = ranCreateBus();
            double distance;
            TimeSpan zman;
                       
            if (index>0)
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
        private static BusLine ranCreateLine(int lineNum)
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
