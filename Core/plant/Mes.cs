namespace Core.Plant
{
  using System.Collections.Generic;
  using Core.Resources;
  using Core.Workcenters;
  using Core.Resources.Virtual;

  public class Mes
  {
    // TODO - Create API for Workcenters to interact with
    // Start wo work
    // End wo work
    // Non conformance
    // Give recommendations??
    // TODO - Connect MES to Plant Scheduler
    // TODO - Connect MES to Transportation Scheduler
    // TODO - Create API for MES to ERP
    // Send process data up? Similar to Workcenter data?
    // Anything else needed?
    // TODO - Connect MES to ERP
    // TODO - API for Machine Breakdowns

    public Mes(string name, Dictionary<string, IAcceptWorkorders> locations)
    {
      Name = name;
      Workorders = new Dictionary<int, VirtualWorkorder>();
      LocationInventories = new Dictionary<string, List<VirtualWorkorder>>();
      Locations = new Dictionary<string, VirtualWorkcenter>();

      foreach(var location in locations)
      {
        var value = location.Value;
        //TODO - Fix Machine Type Issue for creating a Virtual Workcenter... 
        Locations.Add(location.Key, new VirtualWorkcenter(value.Name, value.ListOfValidTypes()));
      }

      foreach(var location in Locations)
      {
        LocationInventories.Add(location.Key, new List<VirtualWorkorder>());
      }
    }

    private Dictionary<string, VirtualWorkcenter> Locations { get; }
    private Dictionary<string, List<VirtualWorkorder>> LocationInventories { get; }
    public string Name { get; }
    private Dictionary<int, VirtualWorkorder> Workorders { get; }

    public void AddWorkorder(string location, IWork wo)
    {
      if(Workorders.ContainsKey(wo.Id))
      {
        throw new System.ArgumentException("Workorder already exists in MES");
      }
      VirtualWorkorder newWo = new VirtualWorkorder(wo.CurrentOpIndex, wo);

      AddWoToLocation(newWo, location);
    }

    public void Complete(int wo_id)
    {
      VirtualWorkorder wo = Workorders[wo_id];
      wo.SetNextOp();
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

    private void AddWoToLocation(VirtualWorkorder wo, string location)
    {
      Workorders.Add(wo.Id, wo);
      LocationInventories[location].Add(wo);
    }

    private void RemoveWoFromLocation(VirtualWorkorder wo, string location)
    {
      LocationInventories[location].Remove(wo);
      Workorders.Remove(wo.Id);
    }
  }
}