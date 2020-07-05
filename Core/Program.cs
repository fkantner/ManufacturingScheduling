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
        private static readonly string filename = Configuration.ResultFileName;
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
            ent.AddTransport(transport);

            Customer customer = new Customer();
            ent.AddCustomer(customer);
            customer.AddEnterprise(ent);

            SimulationNode sn = new SimulationNode(dt, ent);
            WriteJson(sn);

            for(int i = 0; i < Configuration.MinutesForProgramToTest; i++)
            {
                dt.Next();
                ent.Work(dt);
                customer.Work(dt);

		WriteJson(sn);
                if (i%500 == 0) 
                {
                    customer.CreateOrder("p1", new DayTime((int) DayTime.Days.Tue, 800));
                }
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
