namespace Core.Resources
{
  using Plant;
  using System.Collections.Generic;

  public class SimulationNode
  {
    private DayTime _dayTime;
    private IEnumerable<Plant> _plants;

    public SimulationNode(DayTime dayTime, IEnumerable<Plant> plants)
    {
      _dayTime = dayTime;
      _plants = plants;
    }

    public DayTime DayTime { get => _dayTime; }
    public IEnumerable<Plant> Plants { get => _plants; }
  }
}