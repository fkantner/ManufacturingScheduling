namespace Core.Plant
{
  using Core.Workcenters;
  using Core.Resources;
  using System.Collections.Generic;

  public class Dock : IAcceptWorkorders
  {
    public Dock()
    {
      OutputBuffer = new Queue<IWork>();
      ShippingBuffer = new List<IWork>();
      Name = "Shipping Dock";
    }

    public Queue<IWork> OutputBuffer { get; }
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

    public void Work(DayTime dayTime)
    {
      return;
    }
  }
}