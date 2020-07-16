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
    List<IAcceptWorkorders> Workcenters { get; }
    ITransportWork InternalTransportation { get; }
    
    void Add(IEnterprise enterprise);
    void Add(IAcceptWorkorders workcenter);
    void Add(IWork workorder);
    bool CanWorkOnType(Op.OpTypes type);
    IReceive Dock();
    Dictionary<IWork, string> ShipToOtherPlants();
    void Work(DayTime dt);
  }

  public class Plant : IPlant
  {
// Properties
    public ITransportWork InternalTransportation { get; }
    public IMes Mes { get; }
    public string Name { get; }
    public ISchedulePlants PlantScheduler { get; }
    public List<IAcceptWorkorders> Workcenters { get; }
    
// Constructor
    public Plant(string name)
    {
      Name = name;
      Workcenters = new List<IAcceptWorkorders>();
      _dock = new Dock();
      Workcenters.Add(_dock);
      Workcenters.Add(new Stage());
      
      Mes = (IMes) new Mes(name);

      _enterprise = null;

      foreach(IAcceptWorkorders wc in Workcenters)
      {
        wc.SetMes(Mes);
        Mes.Add(wc);
        wc.AddPlant(this);
      }
      

      PlantScheduler = (ISchedulePlants) new PlantScheduler(Mes);
      InternalTransportation = new Transportation(_dock, this);
    }

// Pure Methods
    public bool CanWorkOnType(Op.OpTypes type)
    {
      return Workcenters.Where(x => x.ReceivesType(type)).Any();
    }

    public IReceive Dock()
    {
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
    public void Add(IEnterprise enterprise)
    {
      if(_enterprise != null) { return; }
      _enterprise = enterprise;
    }

    public void Add(IWork workorder)
    {
      var wc = Workcenters.First(x => x.ReceivesType(workorder.CurrentOpType));
      Mes.AddWorkorder(wc.Name, workorder);
      wc.AddToQueue(workorder);
    }

    public void Add(IAcceptWorkorders workcenter)
    {
      Workcenters.Add(workcenter);
      Mes.Add(workcenter);
      workcenter.SetMes(Mes);
      workcenter.AddPlant(this);
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
    private IEnterprise _enterprise;

  }
}