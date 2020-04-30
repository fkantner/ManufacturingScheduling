namespace Core.Resources.Virtual
{
  using Core.Resources;
  using Core.Workcenters;
  using System.Collections.Generic;

  public class VirtualWorkcenter : IAcceptWorkorders
  {
    private readonly List<IWork> _output_buffer;

    public VirtualWorkcenter(string name, string type)
    {
      Name = name;
      Type = type;
      _output_buffer = new List<IWork>();
      InputBuffer = new List<IWork>();
    }

    public List<IWork> InputBuffer { get; }
    public string Name { get; }
    public IEnumerable<IWork> OutputBuffer
    {
      get { return _output_buffer as IEnumerable<IWork>; }
    }
    private string Type { get; }

    public void AddToQueue(IWork wo)
    {
      InputBuffer.Add(wo);
    }

    public string ListOfValidTypes() { return Type; }

    public bool ReceivesType(string type)
    {
      return Type.IndexOf("," + type + ",") > 0;
    }

    public void Work(DayTime dayTime) {}
  }
}