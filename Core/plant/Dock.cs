namespace Core.Plant
{
  using Core.Workcenters;
  using Core.Resources;
  using System.Collections.Generic;

  public class Dock : IAcceptWorkorders
  {
    private IMes _mes;
    public Dock()
    {
      _output_buffer = new Queue<IWork>();
      ShippingBuffer = new List<IWork>();
      Name = "Shipping Dock";
      _mes = null;
    }

    private readonly Queue<IWork> _output_buffer;
    public IEnumerable<IWork> OutputBuffer
    {
      get { return _output_buffer as IEnumerable<IWork>; }
    }
    public List<IWork> ShippingBuffer { get; }
    public string Name { get; }

    public void AddToQueue(IWork workorder)
    {
      _mes?.StopTransit(workorder.Id, Name);
      ShippingBuffer.Add(workorder);
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