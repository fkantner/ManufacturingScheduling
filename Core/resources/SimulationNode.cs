namespace Core.Resources
{
  using Workcenters;

  public class SimulationNode
  {
    private DayTime _dayTime;
    private Workcenter _workcenter;

    public SimulationNode(DayTime dayTime, Workcenter workcenter)
    {
      _dayTime = dayTime;
      _workcenter = workcenter;
    }

    public DayTime DayTime { get => _dayTime; }
    public Workcenter Workcenter { get => _workcenter; }
  }
}