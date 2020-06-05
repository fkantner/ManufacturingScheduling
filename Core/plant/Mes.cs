namespace Core.Plant
{
  using Core;
  using System.Collections.Generic;
  using Enterprise;
  using Resources;
  using Resources.Virtual;
  using Workcenters;

  public enum MesSchedule { DEFAULT=0 };

  public interface IMes
  {
    Dictionary<string, VirtualWorkcenter> Locations { get; }
    Dictionary<string, List<VirtualWorkorder>> LocationInventories { get; }
    Dictionary<int, VirtualWorkorder> Workorders { get; }

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
    // Non conformance
    // TODO - Create API for MES to ERP
    // Send process data up? Similar to Workcenter data?
    // Anything else needed?
    // TODO - Connect MES to ERP
    // TODO - API for Machine Breakdowns
    private MesSchedule _schedule;
    private DayTime _nextDump;

    public Mes(string name, Dictionary<string, IAcceptWorkorders> locations, MesSchedule schedule=(MesSchedule) 0)
    {
      Erp = null;
      Name = name;
      Workorders = new Dictionary<int, VirtualWorkorder>();
      LocationInventories = new Dictionary<string, List<VirtualWorkorder>>();
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
        LocationInventories.Add(location.Key, new List<VirtualWorkorder>());
      }
    }

    private List<Change> Changes { get; }
    private IErp Erp { get; set; }
    public Dictionary<string, VirtualWorkcenter> Locations { get; }
    public Dictionary<string, List<VirtualWorkorder>> LocationInventories { get; }
    public string Name { get; }
    public Dictionary<int, VirtualWorkorder> Workorders { get; }

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
      AddWoToLocation(newWo, location);
      Changes.Add(new Change(newWo.Id, true));
    }

    public void Complete(int wo_id)
    {
      VirtualWorkorder wo = Workorders[wo_id];
      wo.SetNextOp();
      wo.ChangeStatus(VirtualWorkorder.Statuses.Open);
    }

    public List<int> GetLocationWoIds(string location)
    {
      return LocationInventories[location].ConvertAll<int>(x => x.Id);
    }

    public IWork GetWorkorder(int id)
    {
      return Workorders[id];
    }

    public void Move(int wo_id, string source_name, string destination_name)
    {
      bool exists = Workorders.ContainsKey(wo_id);
      if (!exists)
      {
        throw new System.ArgumentOutOfRangeException("Workorder does not exist");
      }
      exists = Locations.ContainsKey(source_name);
      if (!exists)
      {
        throw new System.ArgumentException("Source Location does not exist");
      }
      exists = Locations.ContainsKey(destination_name);
      if (!exists)
      {
        throw new System.ArgumentException("DestinationLocation does not exist");
      }
      VirtualWorkorder wo = Workorders[wo_id];
      RemoveWoFromLocation(wo, source_name);
      AddWoToLocation(wo, destination_name);
    }

    public void Ship(int wo_id)
    {
      VirtualWorkorder wo = Workorders[wo_id];
      Workorders.Remove(wo_id);
      RemoveWoFromLocation(wo, "Shipping Dock");
      Changes.Add(new Change(wo_id, false));
    }

    public void StartProgress(int wo_id)
    {
      Workorders[wo_id].ChangeStatus(VirtualWorkorder.Statuses.InProgress);
    }

    public void StartTransit(int wo_id, string workcenterName)
    {
      VirtualWorkorder wo = Workorders[wo_id];
      wo.ChangeStatus(VirtualWorkorder.Statuses.OnRoute);
      RemoveWoFromLocation(wo, workcenterName);
    }

    public void StopProgress(int wo_id)
    {
      Workorders[wo_id].ChangeStatus(VirtualWorkorder.Statuses.Open);
    }

    public void StopTransit(int wo_id, string workcenterName)
    {
      VirtualWorkorder wo = Workorders[wo_id];
      wo.ChangeStatus(VirtualWorkorder.Statuses.Open);
      if (!LocationInventories[workcenterName].Contains(wo))
      {
        AddWoToLocation(wo, workcenterName);
      }
    }

    public void Work(DayTime dayTime)
    {
      if(_nextDump.Equals(dayTime))
      {
        foreach(Change change in Changes)
        {
          if(change.IsAddToPlant)
          {
            Erp.Receive(woid, Name);
          }
          else
          {
            Erp.Ship(woid, Name);
          }
        }

        IncrementDumpTime();
      }
    }

    private void IncrementDumpTime()
    {
      _nextDump = _schedule switch
      {
          _ => _nextDump.CreateTimestamp(24*60)
      };
    }

    private void AddWoToLocation(VirtualWorkorder wo, string location)
    {
      LocationInventories[location].Add(wo);
    }

    private void RemoveWoFromLocation(VirtualWorkorder wo, string location)
    {
      LocationInventories[location].Remove(wo);
    }

    private class Change
    {
      private int _woid;
      public bool IsAddToPlant { get; }

      public Change(int woid, bool add)
      {
        _woid = woid;
        IsAddToPlant = add;
      }
    }
  }
}