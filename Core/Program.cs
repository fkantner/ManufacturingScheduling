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
    using System.Linq;

    public static class Program
    {
        public static void Main()
        {
            Console.WriteLine("Hello Simulation");
            //List<Test> tests = SimulationSetup.GenerateTests();
            Dictionary<string, float> Results;
            try 
            {
                Results = ResultReader();
            }
            catch
            {
                Results = new Dictionary<string, float>();
            }

            Test Default = new Test("Default", Test.Schedule.DEFAULT, new Dictionary<string, int>());
            Test Match = new Test("Match", Test.Schedule.MATCH, new Dictionary<string, int>());
            RunTest(Default);
            RunTest(Match);

            for(int i = 0; i < 10; i++)
            {
                Test test = SimulationSetup.GenerateInitialTest();

                while(Results.Keys.Any(x => x.Equals(test.Name)))
                {
                    test = SimulationSetup.GenerateInitialTest();
                }

                var result = RunTest(test);
                Results[test.Name] = result;
            }

            SaveResultsToFile(Results);
            Console.WriteLine("Finished with All Simulations");
        }

        static private float RunTest(Test test)
        {
            Stopwatch sw = new Stopwatch();
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

            var result = customer.Result();
            Console.WriteLine("Result: {0}", result.ToString());
            return result;
        }

        static private string ToJson(Object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        static private void SaveToFile(string test, int time, SimulationNode simulationNode)
        {
            string filename = test + "_" + time.ToString() + ".json";
            FileSaver(filename, simulationNode);
        }

        static private void SaveResultsToFile(Dictionary<string, float> results)
        {
            string filename = "finalResults/results.json";
            FileSaver(filename, results);
        }

        static private void FileSaver(string filename, Object obj)
        {
            string path = Configuration.ResultFolder + filename;
            
            if(File.Exists(path))
            {
                File.Delete(path);
            }

            StreamWriter writer = new StreamWriter(path);

            writer.WriteLine(ToJson(obj));

            writer.Dispose();
            writer.Close();
        }

        static private Dictionary<string, float> ResultReader()
        {
            string path = Configuration.ResultFolder + "finalResults/results.json";

            return JsonConvert.DeserializeObject<Dictionary<string, float>>(File.ReadAllText(path));
        }
    }
}
