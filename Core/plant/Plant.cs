namespace Core.Plant
{
  using System.Collections.Generic;
  using System.Linq;
  using Core.Enterprise;
  using Core.Resources;
  using Core.Schedulers;
  using Core.Workcenters;

  public interface IPlant
  {
    IMes Mes { get; }
    string Name { get; }
    ISchedulePlants PlantScheduler { get; }
    IEnumerable<IAcceptWorkorders> Workcenters { get; }
    ITransportWork InternalTransportation { get; set; }
    
    void AddEnterprise(Enterprise enterprise);
    void AddWorkorder(IWork workorder);
    bool CanWorkOnType(string type);
    IReceive Dock();
    Dictionary<IWork, string> ShipToOtherPlants();
    void Work(DayTime dt);
  }

  public class Plant : IPlant
  {
// Properties
    public ITransportWork InternalTransportation { get; set; }
    public IMes Mes { get; }
    public string Name { get; }
    public ISchedulePlants PlantScheduler { get; }
    public IEnumerable<IAcceptWorkorders> Workcenters { get; }
    
// Constructor
    public Plant(string name, IEnumerable<IAcceptWorkorders> workcenters)
    {
      Name = name;
      Workcenters = workcenters;
      _enterprise = null;

      Dictionary<string, IAcceptWorkorders> locations = new Dictionary<string, IAcceptWorkorders>();
      foreach(IAcceptWorkorders wc in Workcenters)
      {
        locations.Add(wc.Name, wc);
        wc.AddPlant(this);
      }
      Mes = (IMes) new Mes(name, locations);

      PlantScheduler = (ISchedulePlants) new PlantScheduler(Mes);

      _dock = null;
      Dock();
    }

// Pure Methods
    public bool CanWorkOnType(string type)
    {
      return Workcenters.Where(x => x.ReceivesType(type)).Any();
    }

    public IReceive Dock()
    {
      if(_dock != null) { return _dock; }
      _dock = (Dock) Workcenters.First(x => x.Name == "Shipping Dock");
      return _dock;
    }

    public Dictionary<IWork, string> ShipToOtherPlants()
    {
      Dictionary<IWork, string> answer = new Dictionary<IWork, string>();
      var dock = Dock();

      int? woid = dock.ShippingBuffer.FirstId();

      while (woid.HasValue)
      {
        IWork wo = dock.Ship(woid.Value);

        string destination = _enterprise.Scheduler.SelectDestinationForExternalTransport(wo.Id);
        answer.Add(wo, destination);

        woid = dock.ShippingBuffer.FirstId();
      }

      return answer;
    }

// Impure Methods
    public void AddEnterprise(Enterprise enterprise)
    {
      if(_enterprise != null) { return; }
      _enterprise = enterprise;
    }

    public void AddWorkorder(IWork workorder)
    {
      var wc = Workcenters.First(x => x.ReceivesType(workorder.CurrentOpType));
      Mes.AddWorkorder(wc.Name, workorder);
      wc.AddToQueue(workorder);
    }

    public void Work( DayTime dt )
    {
      foreach(IAcceptWorkorders wc in Workcenters)
      {
        wc.Work(dt);
      }

      InternalTransportation.Work(dt);

      Mes.Work(dt);
    }

// Private
    private Dock _dock;
    private Enterprise _enterprise;

  }
}