using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace targil2
{
    class Program
    {
        static void Main(string[] args)
        {
            BusLineData buses=new BusLineData();
            BusStop busStop;
            BusStopLine busStopLine;
            BusLine busLine;

            int lineNum;
            double distance;
            TimeSpan zman;

            CHOICE choice; ;
            
            bool success = true;
            do
            {
                Console.WriteLine("Please, make your choice:");
                Console.WriteLine("ADD,REMOVE, FIND,PRINT, EXIT");
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
                                lineNum = createLineNum();
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
                                //buses
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

        private static BusStop createBus()
        {
            Console.WriteLine("enter bus station key:");
            int key = Convert.ToInt32(Console.ReadLine());
            foreach (StopAndCounter station in CounterList.StopAndCounterList)
                if (key == station.stop.BusStationKey)
                        return station.stop;

            Console.WriteLine("enter latitude:");
            double latitude = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("enter longitude:");
            double longitude = Convert.ToDouble(Console.ReadLine());

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
            Console.WriteLine("how many stops does the bus line have?");
            int length = Convert.ToInt32(Console.ReadLine());
            BusLine newLine = new BusLine();

            for (int i = 0; i < length; i++)
            {
                BusStopLine newStop = createBusStopLine(i);
                newLine.add(newStop, i);
            }

            return newLine;
        }
    }
}
