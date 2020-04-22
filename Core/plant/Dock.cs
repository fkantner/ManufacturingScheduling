namespace Core.Plant
{
  using Core.Workcenters;
  using Core.Resources;
  using System.Collections.Generic;

  public class Dock : IAcceptWorkorders
  {
    public Dock()
    {
      _output_buffer = new Queue<IWork>();
      ShippingBuffer = new List<IWork>();
      Name = "Shipping Dock";
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

    public void Work(DayTime dayTime)
    {
      return;
    }
  }
}