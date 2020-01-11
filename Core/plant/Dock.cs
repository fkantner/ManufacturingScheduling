namespace Core.Plant
{
  using Core.Workcenters;
  using Core.Resources;
  using System.Collections.Generic;

  public class Dock : IAcceptWorkorders
  {
    private Queue<Workorder> _buffer;
    private readonly string _name;

    public Dock()
    {
      _buffer = new Queue<Workorder>();
      _name = "Shipping Dock";
    }

    public Queue<Workorder> OutputBuffer { get => _buffer; }
    public string Name { get => _name; }

    public void AddToQueue(Workorder workorder)
    {
      _buffer.Enqueue(workorder);
    }

    public bool ReceivesType(string type)
    {
      return type == "shippingOpType";
    }

    public void Work(DayTime dayTime)
    {
      return;
    }
  }
}