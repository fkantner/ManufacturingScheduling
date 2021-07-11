using System;

namespace simulationCode
{
    using Core;
    using Core.Resources;
    using Core.Plant;
    using Core.Enterprise;
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

            Console.WriteLine("Previous Tests:");
            foreach (var item in Results)
            {
                Console.WriteLine("{0} => {1}", item.Key, item.Value);
            }
/*
            Test Default = new Test("Default", Test.Schedule.DEFAULT, new Dictionary<string, int>());
            Test Match = new Test("Match", Test.Schedule.MATCH, new Dictionary<string, int>());
            RunTest(Default);
            RunTest(Match);
*/
            // Initial Random Tests
            for(int i = 0; i < 100; i++)
            {
                Test test = SimulationSetup.GenerateInitialTest();

                while(Results.Keys.Any(x => x.Equals(test.Name)))
                {
                    test = SimulationSetup.GenerateInitialTest();
                }

                var result = RunTest(test);
                Results[test.Name] = result;
                SaveResultsToFile(Results);
            }

            var rand = new Random();

            var filteredResults = Results.Where((x) => !x.Key.StartsWith("test_6")).ToDictionary(x => x.Key, y => y.Value);

            // Simple Mutate
            for(int i=0; i < 10; i++)
            {
                int t1, t2, t3;
                t1 = rand.Next(filteredResults.Count / 2);
                t2 = rand.Next(filteredResults.Count / 2);
                t3 = rand.Next(filteredResults.Count / 3);
                string n1, n2, n3;

                n1 = GetTest(Results, t1);
                n2 = GetTest(Results, t2);
                n3 = GetTest(Results, t3);

                Test t = SimulationSetup.GenerateMutationTest(n1, n2, n3);
                int count = 0;

                while(Results.Keys.Any(x => x.Equals(t.Name)) && count < 20)
                {
                    t = SimulationSetup.GenerateMutationTest(n1, n2, n3);
                    count++;
                }

                Console.WriteLine("Mutating Tests: {0}, {1}, {2}", n1, n2, n3);

                var result = RunTest(t);
                Results[t.Name] = result;
                SaveResultsToFile(Results);
            }

            // Larger Mutate
            for(int i=0; i < 10; i++)
            {
                int t1, t2, t3, t4;
                t1 = rand.Next(10);
                t2 = rand.Next(20);
                t3 = rand.Next(30);
                t4 = rand.Next(40);

                string n1, n2, n3, n4;

                n1 = GetTest(filteredResults, t1);
                n2 = GetTest(filteredResults, t2);
                n3 = GetTest(filteredResults, t3);
                n4 = GetTest(filteredResults, t4);
                int count = 0;

                Test test;
                do{
                    test = SimulationSetup.GenerateMutationTest(n1, n2, n3, n4);
                    count++;
                } while( Results.Keys.Any(x => x.Equals(test.Name)) && count < 20);

                Console.WriteLine("Mutating Tests:");
                Console.WriteLine("\t{0}", n1);
                Console.WriteLine("\t{0}", n2);
                Console.WriteLine("\t{0}", n3);
                Console.WriteLine("\t{0}", n4);

                var result = RunTest(test);
                Results[test.Name] = result;
                SaveResultsToFile(Results);
            }

            // Find Local Max
            string localTest = GetTest(filteredResults, rand.Next(50));
            Console.WriteLine("Local Test: {0} => {1}", localTest, Results[localTest]);
            Dictionary<string, float> localResults = new Dictionary<string, float>();
            localResults.Add(localTest, Results[localTest]);

            for(int i = 0; i < 12; i++)
            {
                bool atMax = false;
                int counter = 0;
                string lastTest = localTest;
                Test nextTest;
                do {
                    do {
                        nextTest = SimulationSetup.GenerateMutationTest(lastTest, i, 3);
                    } while (Results.Keys.Any(x => x.Equals(nextTest.Name)));
                    
                    var result = RunTest(nextTest);
                    Results[nextTest.Name] = result;
                    localResults[nextTest.Name] = result;
                    atMax = localResults[lastTest] > result;
                    if(localResults[lastTest] == result)
                    {
                        counter++;
                    }
                    else
                    {
                        counter = 0;
                    }
                    lastTest = nextTest.Name;
                } while (atMax == false && counter < 10);

                atMax = false;
                counter = 0;
                lastTest = localTest;
                do {
                    do {
                        nextTest = SimulationSetup.GenerateMutationTest(lastTest, i, -3);
                    } while (Results.Keys.Any(x => x.Equals(nextTest.Name)));
                    
                    var result = RunTest(nextTest);
                    Results[nextTest.Name] = result;
                    localResults[nextTest.Name] = result;
                    atMax = localResults[lastTest] > result;
                    if(localResults[lastTest] == result)
                    {
                        counter++;
                    }
                    else
                    {
                        counter = 0;
                    }
                    lastTest = nextTest.Name;
                } while (atMax == false && counter < 10);
            }

            SaveResultsToFile(Results);
            Console.WriteLine("Finished with All Simulations");
        }

        static private string GetTest(Dictionary<string, float> tests, int rating)
        {
            var list = tests.ToList();
            list.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));
            return list[rating].Key;
        }

        static private float RunTest(Test test)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            DayTime dt = new DayTime();

            Console.WriteLine("Updating Configuration for {0}", test.Name);
            Configuration.Initialize(test);

            //Console.WriteLine("{0}: Creating Customer and Enterprise", test.Name);
            Customer customer = new Customer();
            Enterprise ent = new Enterprise(customer);
            BigData bigData = new BigData();

            //Console.WriteLine("{0}: Generating Plants", test.Name);
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

            //Console.WriteLine("{0}: Generating Transport Routes", test.Name);
            var routes = SimulationSetup.GenerateRoutes(plants);
            Transport transport = new Transport(ent, routes);
            ent.Add(transport);

            //Console.WriteLine("{0}: Generating Simulation Node", test.Name);
            SimulationNode sn = new SimulationNode(dt, ent);
            
            //Console.WriteLine("{0}: Loading Workorders", test.Name);
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

            //SaveToFile(Configuration.Instance.TestFilename, 0, sn);

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
                
                //SaveToFile(Configuration.Instance.TestFilename, i, sn);
            }
            Console.WriteLine(".");
            //Console.WriteLine("Finished with Test {0}", test.Name);
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
