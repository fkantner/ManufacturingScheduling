namespace Core.Plant
{
  using Core.Workcenters;
  using Core.Resources;
  using System.Collections.Generic;

  public class Dock : IAcceptWorkorders
  {
    private Queue<IWork> _output_buffer;
    private List<IWork> _shipping_buffer;
    private readonly string _name;

    public Dock()
    {
      _output_buffer = new Queue<IWork>();
      _shipping_buffer = new List<IWork>();
      _name = "Shipping Dock";
    }

    public Queue<IWork> OutputBuffer { get => _output_buffer; }
    public List<IWork> ShippingBuffer { get => _shipping_buffer; }
    public string Name { get => _name; }

    public void AddToQueue(IWork workorder)
    {
      _shipping_buffer.Add(workorder);
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