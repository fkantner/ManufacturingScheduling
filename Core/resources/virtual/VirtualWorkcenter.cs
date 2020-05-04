namespace Core.Resources.Virtual
{
  using Core.Plant;
  using Core.Resources;
  using Core.Workcenters;
  using System.Collections.Generic;

  public class VirtualWorkcenter : IAcceptWorkorders
  {
    private readonly List<IWork> _output_buffer;

    public VirtualWorkcenter(string name, string types)
    {
      Name = name;
      Types = types;
      _output_buffer = new List<IWork>();
      InputBuffer = new List<IWork>();
    }

    public List<IWork> InputBuffer { get; }
    public string Name { get; }
    public IEnumerable<IWork> OutputBuffer
    {
      get { return _output_buffer as IEnumerable<IWork>; }
    }
    private string Types { get; }

    public void AddToQueue(IWork wo)
    {
      InputBuffer.Add(wo);
    }

    public string ListOfValidTypes() { return Types; }

    public bool ReceivesType(string type)
    {
      return Types.Contains("," + type + ",");
    }

    public void SetMes(IMes mes) { return ; }

    public void Work(DayTime dayTime) {}
  }
}