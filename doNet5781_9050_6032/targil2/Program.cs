using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace targil2
{
    class Program
    {
        static void Main(string[] args)
        {
            BusLineData busLineData=new BusLineData();
            BusStop busStop;
            int line;

            CHOICE choice; ;
            switch (choice)
            {
                case CHOICE.ADD:
                    ADD add;
                    switch (add)
                    {
                        case ADD.ADD_LINE:
                            Console.WriteLine("choose line:");
                            line = Convert.ToInt32(Console.ReadLine());
                            
                            break;
                        case ADD.ADD_BUS_STOP:
                            Console.WriteLine("choose line:");
                            line = Convert.ToInt32(Console.ReadLine());
                            busStop = createBus();
                            Console.WriteLine("which number is the stop?");
                            int index= Convert.ToInt32(Console.ReadLine());
                            //if (index==0)
                            Console.WriteLine("what is the distance from the last stop?");
                            double distance= Convert.ToDouble(Console.ReadLine());
                            Console.WriteLine("how much time past since the last stop?");
                            TimeSpan zman = TimeSpan.Parse(Console.ReadLine());
                            busLineData[line].add(busStop,index,zman,distance);
                            

                            break;
                        default:
                            break;
                    }
                    break;
                case CHOICE.REMOVE:
                    REMOVE remove;
                    switch (remove)
                    {
                        case REMOVE.REMOVE_LINE:
                            break;
                        case REMOVE.REMOVE_BUS_STOP:
                            busStop = createBus();
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
                            busLineData.printLineAtStop(Convert.ToInt32(Console.ReadLine()));
                            break;
                        case FIND.OPTIONS_BETWEEN_STOPS:
                            //int id_first, id_last;
                            //BusLineData temp_buses = new BusLineData;
                            //foreach(BusLine bus in buses)
                            //{
                            //    if()
                            //}

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
    }
}
