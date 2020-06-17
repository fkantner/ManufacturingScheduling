using System;
using Core;

namespace simulationCode
{
    using Core.Resources;
    using Core.Plant;
    using Core.Enterprise;
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

            Console.WriteLine("Generating Plants");
            List<IPlant> plants = SimulationSetup.GeneratePlants();

            Enterprise ent = new Enterprise(dt, plants);
            Console.WriteLine("Generating Transport Routes");
            var routes = SimulationSetup.GenerateRoutes(plants);
            Transport transport = new Transport(ent, routes);

            SimulationNode sn = new SimulationNode(dt, ent);
            WriteJson(sn);

            for(int i = 0; i < 500; i++)
            {
                dt.Next();
                ent.Work(dt);
                transport.Work(dt);
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
