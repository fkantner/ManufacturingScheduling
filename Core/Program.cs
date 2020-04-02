using System;
using Core;

namespace simulationCode
{
    using Core.Resources;
    using Core.Plant;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.IO;

    public static class Program
    {
        private const string filename = "../simulation-ui/src/data/test.json";
        static private bool hasWritten;
        static private StreamWriter writer;

        public static void Main()
        {
            File.Delete(filename);
            writer = new StreamWriter(filename);
            hasWritten = false;
            writer.WriteLine("[");

            Console.WriteLine("Starting Simulation");
            DayTime dt = new DayTime();
            //WriteJson(dt);

            List<Plant> plants = SimulationSetup.GeneratePlants();

            //WriteJson(wc);
            SimulationNode sn = new SimulationNode(dt, plants);
            WriteJson(sn);

            for(int i = 0; i < 500; i++)
            {
                dt.Next();
                plants[0].Work(dt);
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
