using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Targil1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Bus> buses = new List<Bus>();
            CHOICE choice;
            bool success = true;
            do
            {

                success = Enum.TryParse(Console.ReadLine(), out choice);
                if (!success)
                {
                    continue;
                }
                switch (choice)
                {
                    case CHOICE.ADD_BUS:
                        Console.WriteLine("rishuy: ");
                        string rishuy = Console.ReadLine();
                        Console.WriteLine("taarich");
                        DateTime taarich;
                        success = DateTime.TryParse(Console.ReadLine(), out taarich);
                        if (!success)
                        {
                            Console.WriteLine("Error");
                            continue;
                        }
                        try
                        {
                            buses.Add(new Bus(rishuy, taarich));

                        }
                        catch (Exception baaya)
                        {
                            Console.WriteLine(baaya.Message);
                        }
                        //TODO
                        break;
                    case CHOICE.PICK_BUS:
                        break;
                    case CHOICE.REFUEL_BUS:
                        break;
                    case CHOICE.MAINTENANCE_BUS:
                        break;
                    case CHOICE.EXIT:
                        break;
                    default:
                        break;
                }

            } while (choice != CHOICE.EXIT);
        }
    }
}
