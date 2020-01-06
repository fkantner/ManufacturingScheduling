using System;
using Core;

namespace simulationCode
{
    using Core.Resources;
    using Core.Workcenters;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.IO;

    class Program
    {
        static private StreamWriter writer;
        static private readonly string filename = "./simulation-ui/src/data/test.json";
        static private bool hasWritten;
        static void Main(string[] args)
        {
            File.Delete(filename);
            writer = new StreamWriter(filename);
            hasWritten = false;
            writer.WriteLine("[");

            Console.WriteLine("Starting Simulation");
            DayTime dt = new DayTime();
            //WriteJson(dt);

            List<Workorder> wo_list = SimulationSetup.GenerateWorkorders();
            List<Workcenter> wc_list = SimulationSetup.GenerateWorkCenters();

            Workcenter wc = wc_list[0];
            
            foreach(Workorder wo in wo_list)
            {
                wc.AddToQueue(wo);
            }

            //WriteJson(wc);
            SimulationNode sn = new SimulationNode(dt, wc);
            WriteJson(sn);
            
            for(int i = 0; i < 20; i++)
            {
                dt.Next();
                wc.Work(dt);
                WriteJson(sn);
            }

            writer.WriteLine("]");
            writer.Dispose();
            writer.Close();
            Console.WriteLine("Finished with Simulation");
        }

        static private string ToJson(Object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        static private void WriteJson(Object obj)
        {
            if(hasWritten)
            {
                writer.WriteLine("," + ToJson(obj));
            }
            else
            {
                writer.WriteLine(ToJson(obj));
                hasWritten = true;
            }
        }
    }
}
