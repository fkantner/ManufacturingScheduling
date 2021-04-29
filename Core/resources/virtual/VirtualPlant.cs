namespace Core.Resources.Virtual
{
  using Enterprise;
  using Plant;
  using Schedulers;
  using Workcenters;
  using System.Collections.Generic;

  public class VirtualPlant : IPlant
  {
    private Dock _dock;
    private IPlant _original;
    public VirtualPlant(string name, IPlant original)
    {
      Name = name;
      Mes = original.Mes;
      PlantScheduler = original.PlantScheduler;
      InternalTransportation = null;
      
      _original = original;
      _dock = null;

      List<IAcceptWorkorders> temp = new List<IAcceptWorkorders>();
      foreach(var wc in original.Workcenters)
      {
        temp.Add(new VirtualWorkcenter(wc.Name, wc.ListOfValidTypes()));
      }
      Workcenters = temp;
    }

    public IEnterprise Enterprise { get { return null; } }
    public IMes Mes { get; }
    public string Name { get; }
    public ISchedulePlants PlantScheduler { get; }
    public List<IAcceptWorkorders> Workcenters { get; }
    public ITransportWork InternalTransportation { get; set; }

    public IReceive Dock()
    {
      if(_dock != null) { return _dock; }

      foreach(IAcceptWorkorders wc in Workcenters)
      {
        if(wc.Name == "Shipping Dock")
        {
          _dock = (Dock) wc;
          return _dock;
        }
      }

      throw new System.ArgumentOutOfRangeException("No Dock Found!");
    }

    public Dictionary<IWork, string> ShipToOtherPlants()
    {
      // Should never be used...
      return new Dictionary<IWork, string>();
    }

    public void Add(IEnterprise enterprise)
    {
      return; // Don't use
    }

    public void Add(IAcceptWorkorders workcenter)
    {
      // Don't use;
    }

    public void Add(IWork workorder)
    {
      _original.Add(workorder);
      return;
    }

    public bool CanWorkOnType(Op.OpTypes type)
    {
      return _original.CanWorkOnType(type);
    }

    public void Work(DayTime dt)
    {
      return;
    }
  }
}