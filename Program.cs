using System;
using Core;

namespace simulationCode
{
    using Core.Resources;
    using Core.Workcenters;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            DayTime dt = new DayTime();
            WriteJson(dt);

            List<Workorder> wo_list = SimulationSetup.GenerateWorkorders();
            List<Workcenter> wc_list = SimulationSetup.GenerateWorkCenters();

            Workcenter wc = wc_list[0];
            
            foreach(Workorder wo in wo_list)
            {
                wc.AddToQueue(wo);
            }

            WriteJson(wc);

            for(int i = 0; i < 20; i++)
            {
                dt.Next();
                wc.Work(dt);
                WriteJson(dt);
                WriteJson(wc);
            }
        }

        static private string ToJson(Object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        static private void WriteJson(Object obj)
        {
            Console.WriteLine(ToJson(obj));
        }
    }
}
