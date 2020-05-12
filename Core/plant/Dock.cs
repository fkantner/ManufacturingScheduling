namespace Core.Plant
{
  using Core.Workcenters;
  using Core.Resources;

  public class Dock : IAcceptWorkorders
  {
    private IMes _mes;
    public Dock()
    {
      OutputBuffer = new NeoQueue();
      ShippingBuffer = new NeoQueue();
      Name = "Shipping Dock";
      _mes = null;
    }

    public ICustomQueue OutputBuffer { get; }
    public ICustomQueue ShippingBuffer { get; }
    public string Name { get; }

    public void AddToQueue(IWork workorder)
    {
      _mes?.StopTransit(workorder.Id, Name);
      ShippingBuffer.Enqueue(workorder);
    }

    public bool ReceivesType(string type)
    {
      return type == "shippingOp";
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
      return;
    }
  }
}