namespace simulationCode
{
  using Core.Resources;
  using Core.Plant;
  using Core.Workcenters;
  using System.Collections.Generic;

  public static class SimulationSetup
  {
    private const string drillOpType1 = "drillOpType1",
        drillOpType2 = "drillOpType2",
        latheOpType1 = "latheOpType1",
        latheOpType2 = "latheOpType2",
        cncOpType1 = "cncOpType1",
        cncOpType2 = "cncOpType2",
        cncOpType3 = "cncOpType3",
        cncOpType4 = "cncOpType4",
        shippingOpType = "shippingOp";

    public static List<IAcceptWorkorders> GenerateWorkCenters()
    {
      List<IAcceptWorkorders> workcenters = new List<IAcceptWorkorders>();

      string wcName = "drill_WC_a";
      Machine a = new Machine(wcName, new Core.Schedulers.MachineScheduler(), new List<string>{drillOpType1, drillOpType2});
      Workcenter wc = new Workcenter(wcName, a);

      workcenters.Add(wc);

      wcName = "lathe_WC_b";
      Machine b = new Machine(wcName, new Core.Schedulers.MachineScheduler(), new List<string>{latheOpType1, latheOpType2});
      Workcenter wc2 = new Workcenter(wcName, b);

      workcenters.Add(wc2);

      wcName = "cnc_WC_c";
      Machine c = new Machine(wcName, new Core.Schedulers.MachineScheduler(), new List<string>{cncOpType1, cncOpType2});
      Workcenter wc3 = new Workcenter(wcName, c);

      workcenters.Add(wc3);

      wcName = "cnc_WC_d";
      Machine d = new Machine(wcName, new Core.Schedulers.MachineScheduler(), new List<string>{cncOpType2, cncOpType3, cncOpType4});
      Workcenter wc4 = new Workcenter(wcName, d);

      workcenters.Add(wc4);

      Dock dock = new Dock();

      workcenters.Add(dock);

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
      Op shippingOp = new Op(shippingOpType, 0, 0);

      workorders.Add(new Workorder(1, new List<Op> { drillOp1, drillOp2, latheOp1, shippingOp }));
      workorders.Add(new Workorder(2, new List<Op> { drillOp2, drillOp2, drillOp1, latheOp2, shippingOp }));
      workorders.Add(new Workorder(3, new List<Op> { latheOp1, latheOp1, shippingOp }));
      workorders.Add(new Workorder(4, new List<Op> { latheOp2, latheOp2, drillOp1, shippingOp }));
      workorders.Add(new Workorder(5, new List<Op> { latheOp1, latheOp1, drillOp2, shippingOp }));
      workorders.Add(new Workorder(6, new List<Op> { drillOp2, latheOp2, cncOp1, shippingOp }));
      workorders.Add(new Workorder(7, new List<Op> { drillOp1, latheOp1, cncOp2, shippingOp }));
      workorders.Add(new Workorder(8, new List<Op> { latheOp2, latheOp2, cncOp3, shippingOp }));
      workorders.Add(new Workorder(9, new List<Op> { latheOp1, cncOp1, cncOp4, shippingOp }));

      return workorders;
    }

    public static List<Plant> GeneratePlants()
    {
      List<Plant> plants = new List<Plant>();

      List<Workorder> wo_list = SimulationSetup.GenerateWorkorders();
      List<IAcceptWorkorders> wc_list = SimulationSetup.GenerateWorkCenters();

      IAcceptWorkorders wc1 = wc_list[0];
      IAcceptWorkorders wc2 = wc_list[1];
      IAcceptWorkorders wc3 = wc_list[2];
      IAcceptWorkorders wc4 = wc_list[4];

      Plant plant = new Plant("plantA", wc_list);

      foreach(Workorder wo in wo_list)
      {
        if (wc1.ReceivesType(wo.CurrentOpType))
        {
          plant.Mes.AddWorkorder(wc1.Name, wo);
          wc1.AddToQueue(wo);
        }
        else if (wc2.ReceivesType(wo.CurrentOpType))
        {
          plant.Mes.AddWorkorder(wc2.Name, wo);
          wc2.AddToQueue(wo);
        }
        else if (wc3.ReceivesType(wo.CurrentOpType))
        {
          plant.Mes.AddWorkorder(wc3.Name, wo);
          wc3.AddToQueue(wo);
        }
        else if (wc4.ReceivesType(wo.CurrentOpType))
        {
          plant.Mes.AddWorkorder(wc4.Name, wo);
          wc4.AddToQueue(wo);
        }
        else
        {
          throw new System.ArgumentException("No WC for wo: " + wo.ToString());
        }
      }

      plant.InternalTransportation = new Transportation(wc1, new Core.Schedulers.TransportationScheduler(plant));

      plants.Add(plant);

      return plants;
    }
  }
}
