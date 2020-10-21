using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Targil1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Bus> buses = new List<Bus>
            {
               new Bus("2233322",new DateTime(2000,11,11)),
               new Bus("33322333",new DateTime(2020,11,11)),
               new Bus("1133311",new DateTime(2007,01,01)),
               new Bus("33300333",new DateTime(2019,03,28)),
            };
            for (int i = 0; i < 5; i++)
            {
                insertBus(buses);
            }

            CHOICE choice;
            bool success = true;
            do
            {
                Console.WriteLine("Please, make your choce:");
                Console.WriteLine("ADD_BUS, PICK_BUS, REFUEL_BUS, MAINTENANCE_BUS,  EXIT");

                success = Enum.TryParse(Console.ReadLine(), out choice);
                if (!success)
                {
                    continue;
                }
                switch (choice)
                {
                    case CHOICE.ADD_BUS:
                        insertBus(buses);
                        //TODO
                        break;
                    case CHOICE.PICK_BUS:
                        printall(buses);
                        string registration = Console.ReadLine();
                        Bus bus = findBuses(buses, registration);
                        if (bus != null)
                        {
                            Console.WriteLine("the bus is {0} ", bus);
                        }
                        else
                        {
                            Console.WriteLine("ein kaze!!!");
                        }
                        break;
                    case CHOICE.REFUEL_BUS:
                        break;
                    case CHOICE.MAINTENANCE_BUS:
                        break;
                    //case CHOICE.EXIT: //-1
                    //    break;
                    default:
                        break;
                }

            } while (choice != CHOICE.EXIT);
        }

        private static Bus findBuses(List<Bus> buses, string registration)
        {
            registration = registration.Replace("-", string.Empty);

            Bus bus = null;
            foreach (Bus item in buses)
            {
                if (item.Registration == registration)
                {
                    bus = item;
                }
            }
            return bus;
        }

        private static void printall(List<Bus> buses)
        {
            foreach (Bus bus in buses)
            {
                Console.WriteLine(bus);
            }
        }

        private static void insertBus(List<Bus> buses)
        {
            string rishuy;
            DateTime taarich;

            //taarich
            Console.WriteLine("taarich");
            bool success = DateTime.TryParse(Console.ReadLine(), out taarich);
            if (!success)
            {
                Console.WriteLine("Error");
                return;
            }

            //rishuy
            Console.WriteLine("rishuy: ");
            rishuy = Console.ReadLine();
            try
            {
                buses.Add(new Bus(rishuy, taarich));
                printall(buses);
            }
            catch (Exception baaya)
            {
                Console.WriteLine(baaya.Message);
            }

        }
    }
}
