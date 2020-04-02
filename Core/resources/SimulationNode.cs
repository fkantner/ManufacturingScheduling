namespace Core.Resources
{
  using Plant;
  using System.Collections.Generic;

  public class SimulationNode
  {
    public SimulationNode(DayTime dayTime, IEnumerable<Plant> plants)
    {
      DayTime = dayTime;
      Plants = plants;
    }

    public DayTime DayTime { get; }
    public IEnumerable<Plant> Plants { get; }
  }
}