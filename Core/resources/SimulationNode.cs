namespace Core.Resources
{
  using Core.Enterprise;

  public class SimulationNode
  {
    public SimulationNode(DayTime dayTime, Enterprise ent)
    {
      DayTime = dayTime;
      Enterprise = ent;
    }

    public DayTime DayTime { get; }
    public Enterprise Enterprise { get; }
  }
}