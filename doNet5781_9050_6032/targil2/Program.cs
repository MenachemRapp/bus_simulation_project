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


            for (int i = 1; i <= 10||CounterList.StopAndCounterList.Count<40; i++)
            {
                busLine = ranCreateLine(i);
                buses.AddLineBus(busLine);
            }
            for (int i = 0; i < 10; i++)
            {
                int index = rand.Next(1, 9);
                busStopLine = ranCreateBusStopLine(0);
                int mix = rand.Next(1, 10);
                buses[mix].add(busStopLine, index);
                buses[11-mix].add(busStopLine, index);

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
                                buses[lineNum].add(busStopLine,index);
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
                                buses.deleteLine(lineNum);
                                break;
                            case REMOVE.REMOVE_BUS_STOP:
                                busStop = createBus();
                                lineNum = createLineNum();
                                buses[lineNum].remove(busStop);
                                break;
                            default:
                                break;
                        }
                        break;
                    
                    case CHOICE.FIND:
                        FIND find;
                        switch (find)
                        {
                            case FIND.FIND_LINES_IN_STOP:
                                Console.WriteLine("Please enter the station number");
                                buses.printLineAtStop(Convert.ToInt32(Console.ReadLine()));
                                break;
                            case FIND.OPTIONS_BETWEEN_STOPS:
                                int id_first, id_last;
                                BusLineData temp_buses = new BusLineData;
                                foreach (BusLine bus in buses)
                                {
                                    if ()
                            }

                                break;
                            default:
                                break;
                        }
                        break;
                    case CHOICE.PRINT:
                        PRINT print;
                        switch (print)
                        {
                            case PRINT.PRINT_LINES:
                                break;
                            case PRINT.PRINT_STOPS:
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

        private static BusStop createBus()
        {
            Console.WriteLine("enter bus station key:");
            int key = Convert.ToInt32(Console.ReadLine());
            foreach (StopAndCounter station in CounterList.StopAndCounterList)
                if (key == station.stop.BusStationKey)
                        return station.stop;

            double latitude = 31 + 2.3 * rand.NextDouble();
            double longitude = 34.3 + 1.2 * rand.NextDouble();

            Console.WriteLine("enter address");
            string address = Console.ReadLine();

            return new BusStop(key, latitude, longitude, address);
        }

        private static BusStopLine createBusStopLine(int index)
        {
            BusStop busStop = createBus();
            double distance;
            TimeSpan zman;
                       
            if (index>0)
            {
                Console.WriteLine("what is the distance from the last stop?");
                distance = Convert.ToDouble(Console.ReadLine());
                Console.WriteLine("how much time past since the last stop?");
                zman = TimeSpan.Parse(Console.ReadLine());
            }
            else
            {
                distance = 0;
                zman = TimeSpan.Zero;
            }
            return new BusStopLine(busStop, distance, zman);
        }

        private static int createLineNum()
        {
            Console.WriteLine("choose line number:");
            int line = Convert.ToInt32(Console.ReadLine());
            return line;
        }


        private static BusLine createLine()
        {
            BusLine newLine = new BusLine();
            newLine.BusNumber = createLineNum();
            
            Console.WriteLine("how many stops does the bus line have?");
            int length = Convert.ToInt32(Console.ReadLine());
            

            for (int i = 0; i < length; i++)
            {
                BusStopLine newStop = createBusStopLine(i);
                newLine.add(newStop, i);
            }

            Console.WriteLine("Chose an area for the bus line");
            Console.WriteLine("General,Jerusalem,North,South,Center");

           Area area;
           Enum.TryParse(Console.ReadLine(), out area);
            newLine.Area = area;
           return newLine;
        }


        //random
        private static BusStop ranCreateBus()
        {
            int key = rand.Next(1, 1000000);
            foreach (StopAndCounter station in CounterList.StopAndCounterList)
                if (key == station.stop.BusStationKey)
                    return station.stop;

            double latitude = 31 + 2.3 * rand.NextDouble();
            double longitude = 34.3 + 1.2 * rand.NextDouble();

            string address = "";

            return new BusStop(key, latitude, longitude, address);
        }

        private static BusStopLine ranCreateBusStopLine(int index=1)
        {
            BusStop busStop = ranCreateBus();
            double distance;
            TimeSpan zman;
                       
            if (index>0)
            {
                distance = rand.Next() + rand.NextDouble();
                zman = TimeSpan.FromMinutes(rand.Next());
            }
            else
            {
                distance = 0;
                zman = TimeSpan.Zero;
            }
            return new BusStopLine(busStop, distance, zman);
        }

        
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
