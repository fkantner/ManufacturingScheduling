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
            Console.WriteLine("Hello Simulation");
            DayTime dt = new DayTime();

            Console.WriteLine("Creating Customer and Enterprise");
            Customer customer = new Customer();
            Enterprise ent = new Enterprise(customer);

            Console.WriteLine("Generating Plants");
            List<Plant> plants = SimulationSetup.GeneratePlants();
            foreach(Plant plant in plants)
            {
                ent.Add(plant);
            }

            Console.WriteLine("Generating Transport Routes");
            var routes = SimulationSetup.GenerateRoutes(plants);
            Transport transport = new Transport(ent, routes);
            ent.Add(transport);

            Console.WriteLine("Generating Simulation Node");
            SimulationNode sn = new SimulationNode(dt, ent, customer);
            
            Console.WriteLine("Loading Workorders");
            int woCounter = 0;
            while(woCounter < Configuration.InitialNumberOfWorkorders)
            {
                Workorder.PoType type = SimulationSetup.SelectWoPoType(woCounter);
                DayTime due = SimulationSetup.SelectWoDueDate(dt, woCounter);
                int initialOp = SimulationSetup.SelectWoInitialOp(woCounter, Workorder.GetMaxOps(type) - 1);
                customer.CreateOrder(type, due, initialOp);
                woCounter++;
            }
            customer.Work(dt); // Load Workorders into Enterprise

            SaveToFile("default", 0, sn);

            Console.WriteLine("Starting Simulation");
            for(int i = 1; i < Configuration.MinutesForProgramToTest; i++)
            {
                dt.Next();
                ent.Work(dt);
                customer.Work(dt);

                if (i%500 == 0) 
                {
                    Console.Write(".");
                    customer.CreateOrder(Workorder.PoType.p1, new DayTime((int) DayTime.Days.Tue, 800));
                }
                SaveToFile("default", i, sn);
            }
            Console.WriteLine(".");
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

            writer.WriteLine(ToJson(simulationNode));

            writer.Dispose();
            writer.Close();
        }
    }
}
