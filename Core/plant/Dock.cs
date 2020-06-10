namespace Core.Plant
{
  using Core.Workcenters;
  using Core.Resources;

  public interface IReceive : IAcceptWorkorders
  {
    void ReceiveFromExternal(IWork workorder);
    ICustomQueue ShippingBuffer { get; }
    IWork Ship(int wo_id);
  }

  public class Dock : IReceive
  {
    private IMes _mes;
    public Dock()
    {
      OutputBuffer = new NeoQueue();
      ShippingBuffer = new NeoQueue();
      Name = "Shipping Dock";
      _mes = null;
    }

    public ICustomQueue OutputBuffer { get; } // To be picked from plant
    public ICustomQueue ShippingBuffer { get; } // To ship out
    public string Name { get; }

    public void AddPlant(Plant plant)
    {
      // TODO - Implement Dock scheduling.
      return;
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

    public bool ReceivesType(string type)
    {
      return type == "shippingOp";
    }

    public IWork Ship(int wo_id)
    {
      IWork wo = ShippingBuffer.Remove(wo_id);
      _mes.Ship(wo_id);
      return wo;
    }

    public string ListOfValidTypes()
    {
      return "shippingOp";
    }

    public void SetMes(IMes mes)
    {
      if(_mes != null) { return; }
      _mes = mes;
    }

    public void Work(DayTime dayTime)
    {
      // TODO - Implement Dock -> Work
      return;
    }
  }
}