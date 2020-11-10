using System;
using System.Collections.Generic;
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
            CHOICE choice; ;
            switch (choice)
            {
                case CHOICE.ADD:
                    ADD add;
                    switch (add)
                    {
                        case ADD.ADD_LINE:
                            break;
                        case ADD.ADD_BUS_STOP:
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
                            break;
                        case FIND.OPTIONS_BETWEEN_STOPS:
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
    }
}
