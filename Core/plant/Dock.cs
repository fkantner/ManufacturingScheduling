namespace Core.Plant
{
  using Core.Workcenters;
  using Core.Resources;

  public interface IReceive : IAcceptWorkorders
  {
    ICustomQueue ShippingBuffer { get; }
    void ReceiveFromExternal(IWork workorder);
    IWork Ship(int wo_id);
  }

  public class Dock : IReceive
  {
// Properties
    public string Name { get; }
    public ICustomQueue OutputBuffer { get; } // To be picked from plant
    public ICustomQueue ShippingBuffer { get; } // To ship out
    
// Constructor
    public Dock()
    {
      OutputBuffer = new NeoQueue();
      ShippingBuffer = new NeoQueue();
      Name = "Shipping Dock";
      _mes = null;
    }

// Pure Methods
    public string ListOfValidTypes()
    {
      return VALID_TYPE ;
    }

    public bool ReceivesType(string type)
    {
      return type == VALID_TYPE ;
    }

// Impure Methods
    public void AddPlant(Plant plant)
    {
      return; // Noop. Doesn't need to connect to Plant like other WC's.
    }

    public void AddToQueue(IWork workorder)
    {
      _mes?.StopTransit(workorder.Id, Name);
      ShippingBuffer.Enqueue(workorder);
    }

    public void ReceiveFromExternal(IWork workorder)
    {
      OutputBuffer.Enqueue(workorder);
    }

    public void SetMes(IMes mes)
    {
      if(_mes != null) { return; }
      _mes = mes;
    }

    public IWork Ship(int wo_id)
    {
      IWork wo = ShippingBuffer.Remove(wo_id);
      _mes.Ship(wo_id);
      return wo;
    }

    public void Work(DayTime dayTime)
    {
      return; // Noop.
    }

// Private
    private IMes _mes;
    private const string VALID_TYPE = "shippingOp";
    
  }
}