namespace simulationCode
{
  using Core;
  using Core.Resources;
  using Core.Plant;
  using Core.Workcenters;
  using System.Collections.Generic;

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
      const int THREE_PM = 60*15;
      
      var plantCount = plants.Count;

      List<DayTime> routes = new List<DayTime>() 
      {
        new DayTime((int) DayTime.Days.Mon, SIX_AM),
        new DayTime((int) DayTime.Days.Mon, THREE_PM),
        new DayTime((int) DayTime.Days.Tue, SIX_AM),
        new DayTime((int) DayTime.Days.Tue, THREE_PM),
        new DayTime((int) DayTime.Days.Wed, SIX_AM),
        new DayTime((int) DayTime.Days.Wed, THREE_PM),
        new DayTime((int) DayTime.Days.Thu, SIX_AM),
        new DayTime((int) DayTime.Days.Thu, THREE_PM),
        new DayTime((int) DayTime.Days.Fri, SIX_AM),
        new DayTime((int) DayTime.Days.Fri, THREE_PM),
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
     
  }
}
