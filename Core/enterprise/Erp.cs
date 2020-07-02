namespace Core.Enterprise
{
  using Plant;
  using Resources;
  using Resources.Virtual;
  using System;
  using System.Collections.Generic;

  public enum ErpSchedule { DEFAULT=0 };

  public interface IErp
  {
    Dictionary<int, DayTime> DueDates { get; }
    Dictionary<string, VirtualPlant> Locations { get; }
    Dictionary<string, List<VirtualWorkorder>> LocationInventories { get; }
    Dictionary<int, VirtualWorkorder> Workorders { get; }
    void AddWorkorder(string location, IWork work);
    void CreateWorkorder(string type, DayTime due);
    void Receive(int woid, string location);
    void Ship(int woid, string location);
    void Work(DayTime dayTime);
  }

  public class Erp : IErp
  {
    public static readonly List<string> opTypes = new List<string>() {
      "shippingOp",
      "stageOp",
      "drillOpType1",
      "drillOpType2",
      "drillOpType3",
      "latheOpType1",
      "latheOpType2",
      "cncOpType1",
      "cncOpType2",
      "cncOpType3",
      "cncOpType4",
      "pressOpType1",
      "pressOpType2"
    };

    private static readonly List<Op> ops = new List<Op>(){
      new Op(opTypes[0],   0, 0),  //shipping
      new Op(opTypes[1],   0, 0),  //stage
      new Op(opTypes[2],   4, 2),  //drill 1
      new Op(opTypes[3],   6, 3),  //drill 2
      new Op(opTypes[4],   5, 4),  //drill 3
      new Op(opTypes[5],  15, 5),  //lathe 1
      new Op(opTypes[6],  12, 7),  //lathe 2
      new Op(opTypes[7],  30, 10), //cnc 1
      new Op(opTypes[8],  35, 15), //cnc 2
      new Op(opTypes[9],  40, 12), //cnc 3
      new Op(opTypes[10], 45, 10), //cnc 4
      new Op(opTypes[11], 70, 40), //press 1
      new Op(opTypes[12], 90, 55), //press 2
    };

    private static readonly List<List<int>> products = new List<List<int>>{
      new List<int>() { 2, 3, 5 },
      new List<int>() { 3, 3, 2, 6 },
      new List<int>() { 5, 5 },
      new List<int>() { 6, 6, 2 },
      new List<int>() { 5, 5, 3 },
      new List<int>() { 3, 6, 7 },
      new List<int>() { 2, 5, 8 },
      new List<int>() { 6, 6, 9 },
      new List<int>() { 5, 7, 10 },
      new List<int>() { 11, 3, 4 },
      new List<int>() { 12, 4 },
      new List<int>() { 11, 9 }
    };

    public Dictionary<string, VirtualPlant> Locations { get; }
    public Dictionary<string, List<VirtualWorkorder>> LocationInventories { get; }
    public Dictionary<int, VirtualWorkorder> Workorders { get; }
    public Dictionary<int, DayTime> DueDates { get; }
    public List<IWork> NewToSend { get; }

    private int _woCounter;
    private ErpSchedule _schedule;
    private DayTime _nextDump;

    public Erp(string name, IEnumerable<IPlant> plants, ErpSchedule schedule=(ErpSchedule) 0)
    {
      Name = name;
      Locations = new Dictionary<string, VirtualPlant>();
      LocationInventories = new Dictionary<string, List<VirtualWorkorder>>();
      Workorders = new Dictionary<int, VirtualWorkorder>();
      _woCounter = 1;
      _schedule = schedule;
      _nextDump = null;
      DueDates = new Dictionary<int, DayTime>();
      NewToSend = new List<IWork>();

      foreach(var plant in plants)
      {
        Locations.Add(plant.Name, new VirtualPlant(plant.Name, plant));
        LocationInventories.Add(plant.Name, new List<VirtualWorkorder>());
      }
    }

    public DayTime DayTime { get; }
    public string Name { get; }

    public void AddWorkorder(string location, IWork work)
    {
      if(Workorders.ContainsKey(work.Id))
      {
        throw new System.ArgumentException("Workorder already exists in ERP");
      }
      VirtualWorkorder newWo = new VirtualWorkorder(work.CurrentOpIndex, work);
      Workorders[newWo.Id] = newWo;
      AddWoToLocation(newWo, location);
      if (work.Id >= _woCounter)
      {
        _woCounter = work.Id + 1;
      }
    }

    public void CreateWorkorder(string type, DayTime due)
    {
      int productIndex = Int32.Parse(type.Trim('p'));
      List<int> productOpIndexes = products[productIndex];
      List<Op> newOps = new List<Op>();

      newOps.Add(ops[1]); // Stage
      foreach(int productOpIndex in productOpIndexes)
      {
        newOps.Add(ops[productOpIndex]);
      }
      newOps.Add(ops[0]); // Shipping

      Workorder wo = new Workorder(_woCounter, newOps, type);
      _woCounter++;

      AddWorkorder("none", wo);
      DueDates[wo.Id] = due;
      NewToSend.Add(wo);
    }

    public void Receive(int woid, string location)
    {
      LocationInventories[location].Add(Workorders[woid]);
    }

    public void Ship(int woid, string location)
    {
      LocationInventories[location].Remove(Workorders[woid]);
    }
    

    public void Work(DayTime dayTime)
    {
      if (!TimeToCommunicateWithPlants(dayTime)) { return; }

      foreach(Workorder wo in NewToSend)
      {
        foreach(IPlant plant in Locations.Values)
        {
          if(plant.CanWorkOnType(wo.Operations[1].Type))
          {
            // Only adding to Plant. Will update back to ERP from
            // MES later.
            plant.AddWorkorder(wo);
            break;
          }
        }
      }
    }

    private void AddWoToLocation(VirtualWorkorder wo, string location)
    {
      if(location == "none") { return; }
      LocationInventories[location].Add(wo);
    }

    private bool TimeToCommunicateWithPlants(DayTime dayTime)
    {
      if(_nextDump == null) { _nextDump = dayTime.CreateTimestamp(0); }
      
      if(_nextDump.Equals(dayTime))
      {
        IncrementDumpTime();
        return true;
      }

      return false;
    }

    private void IncrementDumpTime()
    {
      _nextDump = _schedule switch
      {
        _ => _nextDump.CreateTimestamp(24*60)
      };
    }
  }
}