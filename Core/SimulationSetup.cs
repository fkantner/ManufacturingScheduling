namespace simulationCode
{
  using Core;
  using Core.Resources;
  using Core.Plant;
  using Core.Workcenters;
  using Core.Schedulers;
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public static class SimulationSetup
  {

    public static List<Plant> GeneratePlants()
    {
      List<Plant> plants = new List<Plant>();

      Plant plantA = new Plant("plantA");
      Plant plantB = new Plant("plantB");

      plantA.Add(new Workcenter("small drill", Machine.Types.SmallDrill));
      plantA.Add(new Workcenter("lathe", Machine.Types.Lathe));
      plantA.Add(new Workcenter("wet cnc", Machine.Types.WetCnc));
      plantA.Add(new Workcenter("dry cnc", Machine.Types.DryCnc));

      plantB.Add(new Workcenter("big drill", Machine.Types.BigDrill));
      plantB.Add(new Workcenter("press", Machine.Types.Press));

      plants.Add(plantA);
      plants.Add(plantB);
      
      return plants;
    }

    public static Dictionary<DayTime, string> GenerateRoutes(List<Plant> plants)
    {
      var answer = new Dictionary<DayTime, string>();
      const int SIX_AM = 60*6;
      const int TEN_AM = 60*10;
      const int THREE_PM = 60*15;
      const int SEVEN_PM = 60*19;
      
      var plantCount = plants.Count;

      List<DayTime> routes = new List<DayTime>() 
      {
        new DayTime((int) DayTime.Days.Sun, SIX_AM),
        new DayTime((int) DayTime.Days.Sun, TEN_AM),
        new DayTime((int) DayTime.Days.Sun, THREE_PM),
        new DayTime((int) DayTime.Days.Sun, SEVEN_PM),
        new DayTime((int) DayTime.Days.Mon, SIX_AM),
        new DayTime((int) DayTime.Days.Mon, TEN_AM),
        new DayTime((int) DayTime.Days.Mon, THREE_PM),
        new DayTime((int) DayTime.Days.Mon, SEVEN_PM),
        new DayTime((int) DayTime.Days.Tue, SIX_AM),
        new DayTime((int) DayTime.Days.Tue, TEN_AM),
        new DayTime((int) DayTime.Days.Tue, THREE_PM),
        new DayTime((int) DayTime.Days.Tue, SEVEN_PM),
        new DayTime((int) DayTime.Days.Wed, SIX_AM),
        new DayTime((int) DayTime.Days.Wed, TEN_AM),
        new DayTime((int) DayTime.Days.Wed, THREE_PM),
        new DayTime((int) DayTime.Days.Wed, SEVEN_PM),
        new DayTime((int) DayTime.Days.Thu, SIX_AM),
        new DayTime((int) DayTime.Days.Thu, TEN_AM),
        new DayTime((int) DayTime.Days.Thu, THREE_PM),
        new DayTime((int) DayTime.Days.Thu, SEVEN_PM),
        new DayTime((int) DayTime.Days.Fri, SIX_AM),
        new DayTime((int) DayTime.Days.Fri, TEN_AM),
        new DayTime((int) DayTime.Days.Fri, THREE_PM),
        new DayTime((int) DayTime.Days.Fri, SEVEN_PM),
        new DayTime((int) DayTime.Days.Sat, SIX_AM),
        new DayTime((int) DayTime.Days.Sat, TEN_AM),
        new DayTime((int) DayTime.Days.Sat, THREE_PM),
        new DayTime((int) DayTime.Days.Sat, SEVEN_PM)
      };

      for(int i=0; i<routes.Count; i++)
      {
        var plant = plants[i%plantCount].Name;
        answer.Add(routes[i], plant);
      }

      return answer;
    }

    public static Workorder.PoType SelectWoPoType(int counter)
    {
      int mod = counter % 13;
      return mod switch
      {
        0 => Workorder.PoType.p0,
        1 => Workorder.PoType.p1,
        2 => Workorder.PoType.p2,
        3 => Workorder.PoType.p3,
        4 => Workorder.PoType.p4,
        5 => Workorder.PoType.p5,
        6 => Workorder.PoType.p6,
        7 => Workorder.PoType.p7,
        8 => Workorder.PoType.p8,
        9 => Workorder.PoType.p9,
        10 => Workorder.PoType.p10,
        11 => Workorder.PoType.p11,
        12 => Workorder.PoType.p12,
        _ => Workorder.PoType.p0
      };
    }

    public static DayTime SelectWoDueDate(DayTime dayTime, int counter)
    {
      int mod = counter % 5;
      int increment = Configuration.MinutesInDay * mod;
      return dayTime.CreateTimestamp(increment);
    }

    public static int SelectWoInitialOp(int counter, int max)
    {
      int answer = 0;
      if(counter % 2 == 0) { answer++; }
      if(counter % 3 == 0) { answer++; }
      if(counter % 5 == 0) { answer++; }
      return answer > max ? max : answer;
    }
     
    public static Test GenerateInitialTest()
    {
      const int limit = 201;
      var rand = new Random();
      List<int> constants = new List<int>();

      for(int i = 0; i<12; i++)
      {
        constants.Add(rand.Next(limit) - 100);
      }

      string testName = CreateTestName(constants);
      
      var test = GenerateTest(testName, constants);
      
      return test;
    }

    public static Test GenerateMutationTest(string parent1, string parent2, string parent3)
    {
      var rand = new Random();
      int arrayLimit = 12;

      List<int> p1 = SplitName(parent1);
      List<int> p2 = SplitName(parent2);
      List<int> p3 = SplitName(parent3);

      int c = rand.Next(arrayLimit);
      p1[c] = (p1[c] + p2[c]) / 2;
      c = rand.Next(arrayLimit);
      p1[c] = (p1[c] + p2[c]) / 2;
      c = rand.Next(arrayLimit);
      p1[c] = (p1[c] + p3[c]) / 2;
      c = rand.Next(arrayLimit);
      p1[c] = (p1[c] + p3[c]) / 2;

      string testName = CreateTestName(p1);
      
      var test = GenerateTest(testName, p1);

      return test;
    }

    public static Test GenerateMutationTest(string parent1, string parent2, string parent3, string parent4)
    {
      var rand = new Random();
      int arrayLimit = 12;

      List<int> p1 = SplitName(parent1);
      List<int> p2 = SplitName(parent2);
      List<int> p3 = SplitName(parent3);
      List<int> p4 = SplitName(parent4);

      int c = rand.Next(arrayLimit);

      int val = (p1[c] + p2[c] + p3[c] + p4[c]) / 4;
      p1[c] = val;

      string testName = CreateTestName(p1);

      return GenerateTest(testName, p1);
    }

    public static Test GenerateMutationTest(string parent, int node, int value)
    {
      List<int> consts = SplitName(parent);

      consts[node] = consts[node] + value;

      string testName = CreateTestName(consts);

      return GenerateTest(testName, consts);
    }

    private static Test GenerateTest(string name, List<int> consts)
    {
      return new Test(name, Test.Schedule.SCHEDULED, new Dictionary<string, int>{
        {"EnterpriseDueDateVariable",         consts[0]},
        {"EnterpriseTravelVariable",          consts[1]},
        {"PlantOperationTimeVariable",        consts[2]},
        {"PlantOperationCountVariable",       consts[3]},
        {"MachineOpTypeVariable",             consts[4]},
        {"MachineWaitTimeVariable",           consts[5]},
        {"MachineDowntimeVariable",           consts[6]},
        {"TransportJobStayVariable",          consts[7]},
        {"TransportJobTransportStayVariable", consts[8]},
        {"TransportWCWaitVariable",           consts[9]},
        {"TransportWCJobCountVariable",       consts[10]},
        {"TransportWCAtCurrentPlantVariable", consts[11]}
      });
    }

    private static string CreateTestName(List<int> cs)
    {
      const string delim = "_";
      string testName = "test_";
      cs.ForEach(x => testName += x.ToString() + delim);
      return testName;
    }

    private static List<int> SplitName(string name)
    {
      const string delim = "_";
      int blah;
      List<int> answer = name.Split(delim).Where(x => int.TryParse(x,out blah)).Select(x => int.Parse(x)).ToList();

      return answer;
    }
  }
}
