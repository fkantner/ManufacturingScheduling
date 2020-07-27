namespace Core.Resources.Virtual
{
  using System.Collections.Generic;
  using Core.Plant;
  using Core.Resources;
  using Core.Workcenters;

  public class VirtualWorkcenter : IAcceptWorkorders
  {
    public VirtualWorkcenter(string name, List<Op.OpTypes> types)
    {
      Name = name;
      Types = types;
      OutputBuffer = new NeoQueue();
      InputBuffer = new List<IWork>();
    }

    public List<IWork> InputBuffer { get; }
    public string Name { get; }
    public ICustomQueue OutputBuffer { get; }
    private List<Op.OpTypes> Types { get; }

    public void AddPlant(IPlant plant)
    { // Doesn't need to respond to Plant.
      return;
    }

    public void AddToQueue(IWork wo)
    {
      InputBuffer.Add(wo);
    }

    public List<Op.OpTypes> ListOfValidTypes() { return Types; }

    public bool ReceivesType(Op.OpTypes type) { return Types.Contains(type); }

    public void SetMes(IMes mes) { return ; }

    public void Work(DayTime dayTime) {}
  }
}