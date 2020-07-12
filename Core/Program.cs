using System;
using Core;

namespace simulationCode
{
    using Core;
    using Core.Resources;
    using Core.Plant;
    using Core.Enterprise;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.IO;

    public static class Program
    {
        public static void Main()
        {
            Console.WriteLine("Starting Simulation");
            DayTime dt = new DayTime();

            Console.WriteLine("Generating Plants");
            List<IPlant> plants = SimulationSetup.GeneratePlants();

            Enterprise ent = new Enterprise(dt, plants);
            Console.WriteLine("Generating Transport Routes");
            var routes = SimulationSetup.GenerateRoutes(plants);
            Transport transport = new Transport(ent, routes);
            ent.AddTransport(transport);

            Customer customer = new Customer();
            ent.AddCustomer(customer);
            customer.AddEnterprise(ent);

            SimulationNode sn = new SimulationNode(dt, ent, customer);
            SaveToFile("default", 0, sn);

            for(int i = 1; i < Configuration.MinutesForProgramToTest; i++)
            {
                dt.Next();
                ent.Work(dt);
                customer.Work(dt);

                if (i%500 == 0) 
                {
                    customer.CreateOrder("p1", new DayTime((int) DayTime.Days.Tue, 800));
                }
                SaveToFile("default", i, sn);
            }

            Console.WriteLine("Finished with Simulation");
        }

        static private string ToJson(Object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        static private void SaveToFile(string test, int time, SimulationNode simulationNode)
        {
            string path = Configuration.ResultFolder;
            string filename = path + test + time.ToString() + ".json";
            if(File.Exists(filename))
            {
                File.Delete(filename);
            }

            StreamWriter writer = new StreamWriter(filename);
            //writer.WriteLine("[");

            writer.WriteLine(ToJson(simulationNode));

            //writer.WriteLine("]");
            writer.Dispose();
            writer.Close();
        }
    }
}
