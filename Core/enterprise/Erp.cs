namespace Core.Enterprise
{
  using System;
  using System.Collections.Generic;
  using Plant;
  using Resources;
  using Resources.Virtual;

  public enum ErpSchedule { DEFAULT=0 };

  public interface IErp
  {
    Dictionary<int, DayTime> DueDates { get; }
    Dictionary<string, List<VirtualWorkorder>> LocationInventories { get; }
    Dictionary<string, VirtualPlant> Locations { get; }
    Dictionary<int, VirtualWorkorder> Workorders { get; }
    void Add(IPlant plant);
    void AddWorkorder(string location, IWork work);
    void CreateWorkorder(Workorder.PoType type, DayTime due, int initialOp=0);
    void Receive(int woid, string location);
    void Ship(int woid, string location);
    void Work(DayTime dayTime);
  }

  public class Erp : IErp
  {
// Properties
    public DayTime DayTime { get; }
    public Dictionary<int, DayTime> DueDates { get; }
    public Dictionary<string, List<VirtualWorkorder>> LocationInventories { get; }
    public Dictionary<string, VirtualPlant> Locations { get; }
    public string Name { get; }
    public List<IWork> NewToSend { get; }
    public Dictionary<int, VirtualWorkorder> Workorders { get; }
    
// Constructor
    public Erp(string name, ErpSchedule schedule=(ErpSchedule) 0)
    {
      Name = name;
      Locations = new Dictionary<string, VirtualPlant>();
      LocationInventories = new Dictionary<string, List<VirtualWorkorder>>();
      Workorders = new Dictionary<int, VirtualWorkorder>();
      _schedule = schedule;
      _nextDump = new DayTime();
      DueDates = new Dictionary<int, DayTime>();
      NewToSend = new List<IWork>();
    }
// Pure Methods

// Impure Methods
    public void Add(IPlant plant)
    {
      Locations.Add(plant.Name, new VirtualPlant(plant.Name, plant));
      LocationInventories.Add(plant.Name, new List<VirtualWorkorder>());
    }

    public void AddWorkorder(string location, IWork work)
    {
      if(Workorders.ContainsKey(work.Id))
      {
        throw new System.ArgumentException("Workorder already exists in ERP");
      }

      VirtualWorkorder newWo = new VirtualWorkorder(work.CurrentOpIndex, work);
      Workorders[newWo.Id] = newWo;

      if(location == "none") { return; }
      LocationInventories[location].Add(newWo);
    }

    public void CreateWorkorder(Workorder.PoType type, DayTime due, int initialOp=0)
    {
      Workorder wo = new Workorder(GetNextWoId(), type, initialOp);
      
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
      if (!IsTimeToCommunicateWithPlants(dayTime)) { return; }

      _nextDump = NextDumpTime(dayTime);

      foreach(Workorder wo in NewToSend)
      {
        foreach(IPlant plant in Locations.Values)
        {
          int importantOp = 1;
          if(wo.CurrentOpIndex > importantOp){ importantOp = wo.CurrentOpIndex; }

          if(plant.CanWorkOnType(wo.Operations[importantOp].Type))
          {
            // Only adding to Plant. Will update back to ERP from
            // MES later.
            plant.Add(wo);
            break;
          }
        }
      }
      NewToSend.Clear();
    }

// Private
    
    private DayTime _nextDump;
    private ErpSchedule _schedule;

// private pure
    private DayTime NextDumpTime(DayTime currentDumpTime)
    {
      //TODO Create configuration for ERP to update MES's
      return _schedule switch
      {
        _ => currentDumpTime.CreateTimestamp(24*60)
      };
    }

    private bool IsTimeToCommunicateWithPlants(DayTime dayTime)
    {
      return _nextDump.Equals(dayTime) || _nextDump.LessThan(dayTime);
    }
    
    private int GetNextWoId()
    {
      int highest = 0;
      
      foreach(int key in Workorders.Keys)
      {
        if (key > highest) { highest = key; }
      }

      return highest + 1;
    }

  }
}