namespace Core.Plant
{
  using System.Collections.Generic;
  using Core;
  using Enterprise;
  using Resources;
  using Resources.Virtual;
  using Workcenters;

  // TODO Non conformance
  // TODO - API for Machine Breakdowns
  public enum MesSchedule { DEFAULT=0 };

  public interface IMes
  {
    Dictionary<string, List<IWork>> LocationInventories { get; }
    Dictionary<string, VirtualWorkcenter> Locations { get; }
    Dictionary<int, IWork> Workorders { get; }

    void AddErp(IErp erp);
    void AddWorkorder(string location, IWork wo);
    void Complete(int wo_id);
    List<int> GetLocationWoIds(string location);
    IWork GetWorkorder(int id);
    void Move(int wo_id, string source_name, string destination_name);
    void Ship(int wo_id);
    void StartProgress(int wo_id);
    void StartTransit(int wo_id, string workcenterName);
    void StopProgress(int wo_id);
    void StopTransit(int wo_id, string workcenterName);
    void Work(DayTime dayTime);
  }

  public class Mes : IMes
  {
// Properties
    public Dictionary<string, List<IWork>> LocationInventories { get; }
    public Dictionary<string, VirtualWorkcenter> Locations { get; }
    public string Name { get; }
    public Dictionary<int, IWork> Workorders { get; }

// Constructor
    public Mes(string name, Dictionary<string, IAcceptWorkorders> locations, MesSchedule schedule=(MesSchedule) 0)
    {
      Erp = null;
      Name = name;
      Workorders = new Dictionary<int, IWork>();
      LocationInventories = new Dictionary<string, List<IWork>>();
      Locations = new Dictionary<string, VirtualWorkcenter>();
      Changes = new List<Change>();

      _schedule = schedule;
      _nextDump = new DayTime();

      foreach(var location in locations)
      {
        var value = location.Value;
        Locations.Add(location.Key, new VirtualWorkcenter(value.Name, value.ListOfValidTypes()));
        location.Value.SetMes(this);
      }

      foreach(var location in Locations)
      {
        LocationInventories.Add(location.Key, new List<IWork>());
      }
    }

// Pure Methods
    public List<int> GetLocationWoIds(string location)
    {
      return LocationInventories[location].ConvertAll<int>(x => x.Id);
    }
    
    public IWork GetWorkorder(int id)
    {
      return Workorders[id];
    }

// Impure Methods
    public void AddErp(IErp erp)
    {
      if(Erp != null) { return; }
      Erp = erp;
    }

    public void AddWorkorder(string location, IWork wo)
    {
      if(Workorders.ContainsKey(wo.Id))
      {
        throw new System.ArgumentException("Workorder already exists in MES");
      }
      VirtualWorkorder newWo = new VirtualWorkorder(wo.CurrentOpIndex, wo);
      
      Workorders[newWo.Id] = newWo;
      LocationInventories[location].Add(newWo);
      Changes.Add(new Change(newWo.Id, true));
    }

    public void Complete(int wo_id)
    {
      VirtualWorkorder wo = (VirtualWorkorder) Workorders[wo_id];
      wo.SetNextOp();
      wo.ChangeStatus(VirtualWorkorder.Statuses.Open);
    }

    public void Move(int wo_id, string source_name, string destination_name)
    {
      if (!Workorders.ContainsKey(wo_id))
      {
        throw new System.ArgumentOutOfRangeException("Workorder does not exist");
      }
      
      if (!Locations.ContainsKey(source_name))
      {
        throw new System.ArgumentException("Source Location does not exist");
      }
      
      if (!Locations.ContainsKey(destination_name))
      {
        throw new System.ArgumentException("Destination Location does not exist");
      }

      VirtualWorkorder wo = (VirtualWorkorder) Workorders[wo_id];
      LocationInventories[source_name].Remove(wo);
      LocationInventories[destination_name].Add(wo);
    }

    public void Ship(int wo_id)
    {
      VirtualWorkorder wo = (VirtualWorkorder) Workorders[wo_id];
      Workorders.Remove(wo_id);
      LocationInventories["Shipping Dock"].Remove(wo);
      Changes.Add(new Change(wo_id, false));
    }

    public void StartProgress(int wo_id)
    {
      ((VirtualWorkorder) Workorders[wo_id]).ChangeStatus(VirtualWorkorder.Statuses.InProgress);
    }

    public void StartTransit(int wo_id, string workcenterName)
    {
      VirtualWorkorder wo = (VirtualWorkorder) Workorders[wo_id];
      wo.ChangeStatus(VirtualWorkorder.Statuses.OnRoute);
      LocationInventories[workcenterName].Remove(wo);
    }

    public void StopProgress(int wo_id)
    {
      ((VirtualWorkorder) Workorders[wo_id]).ChangeStatus(VirtualWorkorder.Statuses.Open);
    }

    public void StopTransit(int wo_id, string workcenterName)
    {
      VirtualWorkorder wo = (VirtualWorkorder) Workorders[wo_id];
      wo.ChangeStatus(VirtualWorkorder.Statuses.Open);
      if (!LocationInventories[workcenterName].Contains(wo))
      {
        LocationInventories[workcenterName].Add(wo);
      }
    }

    public void Work(DayTime dayTime)
    {
      if(!_nextDump.Equals(dayTime)) { return; }
      
      foreach(Change change in Changes)
      {
        if(change.IsAddToPlant)
        {
          Erp.Receive(change.Woid, Name);
        }
        else
        {
          Erp.Ship(change.Woid, Name);
        }
      }

      _nextDump = NextDumpTime(dayTime);
    }

// Private
    private DayTime _nextDump;
    private MesSchedule _schedule;
    private List<Change> Changes { get; }
    private IErp Erp { get; set; }

    private DayTime NextDumpTime(DayTime currentDumpTime)
    {
      return _schedule switch
      {
        _ => currentDumpTime.CreateTimestamp(24*60)
      };
    }

    private class Change
    {
      public int Woid { get; }
      public bool IsAddToPlant { get; }

      public Change(int woid, bool add)
      {
        Woid = woid;
        IsAddToPlant = add;
      }
    }
  }
}