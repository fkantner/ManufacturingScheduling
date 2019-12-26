using System;
using Core;

namespace simulationCode
{
    using Core.Resources;
    using Core.Workcenters;
    using System.Collections.Generic;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            DayTime dt = new DayTime();
            Console.WriteLine(dt.ToString());

            // Create WorkOrders
            List<Op> ops = new List<Op>();

            Core.Resources.Op op1 = new Core.Resources.Op("testop1", 4, 1);
            Core.Resources.Op op2 = new Core.Resources.Op("testop2", 5, 2);
            Core.Resources.Op op3 = new Core.Resources.Op("testop3", 7, 1);
            ops.Add(op1); ops.Add(op2); ops.Add(op3);

            Workorder wo = new Workorder(1, ops);        

            Console.WriteLine(wo.ToString());

            for(int i = 0; i < 1500; i++)
            {
                dt.Next();
                //Console.WriteLine(dt.ToString());
            }
        }
    }
}
