namespace simulationCode
{
  using Core.Resources;
  using Core.Workcenters;
  using System.Collections.Generic;

  public class SimulationSetup
  {
    static readonly string drill = "Drill", lathe = "Lathe", cnc = "CNC";
    static readonly string drillOpType1 = "drillOpType1", 
        drillOpType2 = "drillOpType2", 
        latheOpType1 = "latheOpType1", 
        latheOpType2 = "latheOpType2",
        cncOpType1 = "cncOpType1",
        cncOpType2 = "cncOpType2",
        cncOpType3 = "cncOpType3",
        cncOpType4 = "cncOpType4";

    public static List<Workcenter> GenerateWorkCenters()
    {

      List<Workcenter> workcenters = new List<Workcenter>();

      Machine a = new Machine(drill, new Core.Schedulers.MachineScheduler(), new List<string>{drillOpType1, drillOpType2});
      Workcenter wc = new Workcenter("drill_WC_a", a);

      workcenters.Add(wc);

      return workcenters;
    }

    public static List<Workorder> GenerateWorkorders()
    {
      List<Workorder> workorders = new List<Workorder>();

      Op drillOp1 = new Op(drillOpType1, 4, 1);
      Op drillOp2 = new Op(drillOpType2, 5, 1);
      Op latheOp1 = new Op(latheOpType1, 15, 5);
      Op latheOp2 = new Op(latheOpType2, 13, 5);
      Op cncOp1 = new Op(cncOpType1, 30, 10);
      Op cncOp2 = new Op(cncOpType2, 35, 15);
      Op cncOp3 = new Op(cncOpType3, 40, 12);
      Op cncOp4 = new Op(cncOpType4, 45, 10);

      workorders.Add(new Workorder(1, new List<Op> { drillOp1, drillOp2, drillOp1 }));
      workorders.Add(new Workorder(2, new List<Op> { drillOp2, drillOp2, drillOp1 }));

      return workorders;
    }
    
  }
}
