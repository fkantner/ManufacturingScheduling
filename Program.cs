using System;
using Core;

namespace simulationCode
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            DayTime dt = new DayTime();
            Console.WriteLine(dt.ToString());

            for(int i = 0; i < 1500; i++)
            {
                dt.Next();
                Console.WriteLine(dt.ToString());
            }
        }
    }
}
