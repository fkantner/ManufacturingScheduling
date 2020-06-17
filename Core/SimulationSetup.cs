namespace simulationCode
{
  using Core;
  using Core.Resources;
  using Core.Plant;
  using Core.Workcenters;
  using System.Collections.Generic;

  public static class SimulationSetup
  {
    private const string drillOpType1 = "drillOpType1",
        drillOpType2 = "drillOpType2",
        drillOpType3 = "drillOpType3",
        latheOpType1 = "latheOpType1",
        latheOpType2 = "latheOpType2",
        cncOpType1 = "cncOpType1",
        cncOpType2 = "cncOpType2",
        cncOpType3 = "cncOpType3",
        cncOpType4 = "cncOpType4",
        pressOpType1 = "pressOpType1",
        pressOpType2 = "pressOpType2",
        shippingOpType = "shippingOp",
        stageOpType = "stageOp";

    public static List<IAcceptWorkorders> GenerateWorkCenters(bool isPlantA)
    {
      List<IAcceptWorkorders> workcenters = new List<IAcceptWorkorders>();

      workcenters.Add(new Stage());
      
      if(isPlantA)
      {
        workcenters.Add(GenerateWC("drill_WC_a", new List<string>{drillOpType2, drillOpType3}));
        workcenters.Add(GenerateWC("lathe_WC_b", new List<string>{latheOpType1, latheOpType2}));
        workcenters.Add(GenerateWC("cnc_WC_c", new List<string>{cncOpType1, cncOpType2}));
        workcenters.Add(GenerateWC("cnc_WC_d", new List<string>{cncOpType2, cncOpType3, cncOpType4}));
      }
      else
      {
        workcenters.Add(GenerateWC("drill_WC_e", new List<string>{drillOpType1, drillOpType2}));
        workcenters.Add(GenerateWC("press_WC_f", new List<string>{pressOpType1, pressOpType2}));
      }
      
      workcenters.Add(new Dock());

      return workcenters;
    }

    private static Workcenter GenerateWC(string name, List<string> opTypes)
    {
      Machine m = new Machine(name, new Core.Schedulers.MachineScheduler(), opTypes);
      return new Workcenter(name, m);
    }

    public static List<Workorder> GenerateWorkorders()
    {
      List<Workorder> workorders = new List<Workorder>();
      var generator = new WorkorderGenerator();

      for(int i=0; i<40; i++)
      {
        workorders.Add(generator.GenerateWorkorder());
      }

      return workorders;
    }

    public static List<IPlant> GeneratePlants()
    {
      List<IPlant> plants = new List<IPlant>();

      List<Workorder> wo_list = GenerateWorkorders();
      List<IAcceptWorkorders> wc_listA = GenerateWorkCenters(true);
      List<IAcceptWorkorders> wc_listB = GenerateWorkCenters(false);

      IPlant plantA = new Plant("plantA", wc_listA);
      IPlant plantB = new Plant("plantB", wc_listB);

      plantA.InternalTransportation = new Transportation(wc_listA[0], new Core.Schedulers.TransportationScheduler(plantA));
      plantB.InternalTransportation = new Transportation(wc_listB[0], new Core.Schedulers.TransportationScheduler(plantB));

      plants.Add(plantA);
      plants.Add(plantB);

      foreach(Workorder wo in wo_list)
      {
        bool placed = false;

        foreach(IAcceptWorkorders wc in wc_listA)
        {
          if(wc.ReceivesType(wo.CurrentOpType))
          {
            plantA.Mes.AddWorkorder(wc.Name, wo);
            wc.AddToQueue(wo);
            placed = true;
            break;
          }
        }
        if(placed) { continue; }

        foreach(IAcceptWorkorders wc in wc_listB)
        {
          if(wc.ReceivesType(wo.CurrentOpType))
          {
            plantB.Mes.AddWorkorder(wc.Name, wo);
            wc.AddToQueue(wo);
            placed = true;
            break;
          }
        }

        if(!placed)
        {
          throw new System.ArgumentException("No WC for wo: " + wo.ToString());
        }
      }

      return plants;
    }

    public static Dictionary<DayTime, string> GenerateRoutes(List<IPlant> plants)
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

    private class WorkorderGenerator
    {
      private static int counter;
      private static readonly Op drillOp1 = new Op(drillOpType1, 4, 2);
      private static readonly Op drillOp2 = new Op(drillOpType2, 6, 3);
      private static readonly Op drillOp3 = new Op(drillOpType3, 5, 4);
      private static readonly Op latheOp1 = new Op(latheOpType1, 15, 5);
      private static readonly Op latheOp2 = new Op(latheOpType2, 12, 7);
      private static readonly Op cncOp1 = new Op(cncOpType1, 30, 10);
      private static readonly Op cncOp2 = new Op(cncOpType2, 35, 15);
      private static readonly Op cncOp3 = new Op(cncOpType3, 40, 12);
      private static readonly Op cncOp4 = new Op(cncOpType4, 45, 10);
      private static readonly Op pressOp1 = new Op(pressOpType1, 70, 40);
      private static readonly Op pressOp2 = new Op(pressOpType2, 90, 55);
      private static readonly Op shippingOp = new Op(shippingOpType, 0, 0);
      private static readonly Op stagingOp = new Op(stageOpType, 0, 0);

      private static readonly List<Op> p1 = new List<Op> { stagingOp, drillOp1, drillOp2, latheOp1, shippingOp };
      private static readonly List<Op> p2 = new List<Op> { stagingOp, drillOp2, drillOp2, drillOp1, latheOp2, shippingOp};
      private static readonly List<Op> p3 = new List<Op> { stagingOp, latheOp1, latheOp1, shippingOp };
      private static readonly List<Op> p4 = new List<Op> { stagingOp, latheOp2, latheOp2, drillOp1, shippingOp };
      private static readonly List<Op> p5 = new List<Op> { stagingOp, latheOp1, latheOp1, drillOp2, shippingOp };
      private static readonly List<Op> p6 = new List<Op> { stagingOp, drillOp2, latheOp2, cncOp1, shippingOp };
      private static readonly List<Op> p7 = new List<Op> { stagingOp, drillOp1, latheOp1, cncOp2, shippingOp };
      private static readonly List<Op> p8 = new List<Op> { stagingOp, latheOp2, latheOp2, cncOp3, shippingOp };
      private static readonly List<Op> p9 = new List<Op> { stagingOp, latheOp1, cncOp1, cncOp4, shippingOp };
      private static readonly List<Op> p10 = new List<Op> { stagingOp, pressOp1, drillOp2, drillOp3, shippingOp };
      private static readonly List<Op> p11 = new List<Op> { stagingOp, pressOp2, drillOp3, shippingOp };
      private static readonly List<Op> p12 = new List<Op> { stagingOp, pressOp1, cncOp3, shippingOp };

      public WorkorderGenerator()
      {
        counter = 1;
      }

      public Workorder GenerateWorkorder()
      {
        List<Op> p = SelectP();
        Workorder answer = new Workorder(counter, p);
        if(counter % 3 == 0) { answer.SetNextOp(); }
        if(counter % 5 == 0) { answer.SetNextOp(); }
        counter++;
        return answer;
      }

      private List<Op> SelectP()
      {
        int flag = counter % 12;
        var p = flag switch
        {
          0 => p1,
          1 => p2,
          2 => p3,
          3 => p4,
          4 => p5,
          5 => p6,
          6 => p7,
          7 => p8,
          8 => p9,
          9 => p10,
          10 => p11,
          11 => p12,
          _ => p1
        };
        return p;
      }
    }
  }
}
