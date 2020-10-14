using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doNet5781_9050_6032
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome9050();
            Wellcome6032();
            
        }

        private static void Welcome9050()
        {
            Console.Write("Enter your name: ");
            String userName = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", userName);
        }
        static partial void Wellcome6032();
    }
}