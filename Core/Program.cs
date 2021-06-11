using System;

namespace simulationCode
{
    using Core;
    using Core.Resources;
    using Core.Plant;
    using Core.Enterprise;
    using Core.Schedulers;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;

    public static class Program
    {
        public static void Main()
        {
            Console.WriteLine("Hello Simulation");
            List<Test> tests = SimulationSetup.GenerateTests();
            Stopwatch sw = new Stopwatch();

            tests.ForEach( test =>
            {
                sw.Start();
                DayTime dt = new DayTime();

                Console.WriteLine("Updating Configuration for {0}", test.Name);
                Configuration.Initialize(test);

                Console.WriteLine("{0}: Creating Customer and Enterprise", test.Name);
                Customer customer = new Customer();
                Enterprise ent = new Enterprise(customer);
                BigData bigData = new BigData();

                Console.WriteLine("{0}: Generating Plants", test.Name);
                List<Plant> plants = SimulationSetup.GeneratePlants();
                foreach(Plant plant in plants)
                {
                    ent.Add(plant);
                    foreach(var wc in plant.Workcenters)
                    {
                        if(wc.Name == "Shipping Dock" || wc.Name == "Stage") { continue; }
                        bigData.AddWorkcenter(wc.Name);
                        ((Core.Workcenters.Workcenter) wc).AddBigData(bigData);
                    }
                }
                ent.Add(bigData);

                Console.WriteLine("{0}: Generating Transport Routes", test.Name);
                var routes = SimulationSetup.GenerateRoutes(plants);
                Transport transport = new Transport(ent, routes);
                ent.Add(transport);

                Console.WriteLine("{0}: Generating Simulation Node", test.Name);
                SimulationNode sn = new SimulationNode(dt, ent);
                
                Console.WriteLine("{0}: Loading Workorders", test.Name);
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

                SaveToFile(Configuration.Instance.TestFilename, 0, sn);

                Console.WriteLine("{0}: Starting Simulation", test.Name);
                for(int i = 1; i < Configuration.MinutesForProgramToTest; i++)
                {
                    dt.Next();
                    ent.Work(dt);
                    customer.Work(dt);

                    var next = bigData.GetNextOrder(i);
                    if(next.HasValue)
                    {
                        customer.CreateOrder(next.Value.Item1, new DayTime((int) next.Value.Item2, 800));
                    }

                    if (i%500 == 0) 
                    {
                        Console.Write(".");
                    }
                    
                    SaveToFile(Configuration.Instance.TestFilename, i, sn);
                }
                Console.WriteLine(".");
                Console.WriteLine("Finished with Test {0}", test.Name);
                sw.Stop();
                Console.WriteLine("Time to Complete: {0}", sw.Elapsed);
                sw.Reset();
            });

            Console.WriteLine("Finished with All Simulations");
        }

        static private string ToJson(Object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        static private void SaveToFile(string test, int time, SimulationNode simulationNode)
        {
            string path = Configuration.ResultFolder;
            string filename = path + test + "_" + time.ToString() + ".json";
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
