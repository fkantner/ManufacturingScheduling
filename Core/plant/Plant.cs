namespace Core.Plant
{
  using System.Collections.Generic;
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
    IReceive Dock();
    Dictionary<IWork, string> ShipToOtherPlants();
    void AddEnterprise(Enterprise enterprise);
    bool CanWorkOnType(string type);
    void Work(DayTime dt);
  }

  public class Plant : IPlant
  {
    private Enterprise _enterprise;

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
      Mes = (IMes) new Mes("MES", locations);

      PlantScheduler = (ISchedulePlants) new PlantScheduler(Mes, PlantSchedule.DEFAULT);

      _dock = null;
      Dock();
    }

    public IMes Mes { get; }
    public string Name { get; }
    public ISchedulePlants PlantScheduler { get; }
    public IEnumerable<IAcceptWorkorders> Workcenters { get; }
    public ITransportWork InternalTransportation { get; set; }
    private Dock _dock;

    public void AddEnterprise(Enterprise enterprise)
    {
      if(_enterprise == null) { return; }
      _enterprise = enterprise;
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

    public bool CanWorkOnType(string type)
    {
      foreach(var workcenter in Workcenters)
      {
        if(workcenter.ReceivesType(type))
        {
          return true;
        }
      }
      return false;
    }

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

    public void Work( DayTime dt )
    {
      foreach(IAcceptWorkorders wc in Workcenters)
      {
        wc.Work(dt);
      }

      InternalTransportation.Work(dt);

      Mes.Work(dt);
    }
  }
}