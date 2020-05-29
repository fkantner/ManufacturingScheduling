namespace Core.Enterprise
{
  using Plant;
  using Resources;
  using Resources.Virtual;
  using System.Collections.Generic;

  public interface IErp
  {
    Dictionary<string, VirtualPlant> Locations { get; }
    Dictionary<string, List<VirtualWorkorder>> LocationInventories { get; }
    Dictionary<int, VirtualWorkorder> Workorders { get; }
    void AddWorkorder(string location, IWork work);
    void Work(DayTime dayTime);
  }

  public class Erp : IErp
  {
    public Dictionary<string, VirtualPlant> Locations { get; }
    public Dictionary<string, List<VirtualWorkorder>> LocationInventories { get; }
    public Dictionary<int, VirtualWorkorder> Workorders { get; }

    public Erp(string name, IEnumerable<IPlant> plants)
    {
      Name = name;
      Locations = new Dictionary<string, VirtualPlant>();
      LocationInventories = new Dictionary<string, List<VirtualWorkorder>>();
      Workorders = new Dictionary<int, VirtualWorkorder>();

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
    }
    public void Work(DayTime dayTime)
    {
      // Do Something
    }

    private void AddWoToLocation(VirtualWorkorder wo, string location)
    {
      LocationInventories[location].Add(wo);
    }
  }
}