namespace Core.Plant
{
  using Core.Workcenters;
  using Core.Resources;
  using System.Collections.Generic;

  public class Dock : IAcceptWorkorders
  {
    private Queue<Workorder> _output_buffer;
    private List<Workorder> _shipping_buffer;
    private readonly string _name;

    public Dock()
    {
      _output_buffer = new Queue<Workorder>();
      _shipping_buffer = new List<Workorder>();
      _name = "Shipping Dock";
    }

    public Queue<Workorder> OutputBuffer { get => _output_buffer; }
    public List<Workorder> ShippingBuffer { get => _shipping_buffer; }
    public string Name { get => _name; }

    public void AddToQueue(Workorder workorder)
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