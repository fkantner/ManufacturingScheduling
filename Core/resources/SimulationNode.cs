namespace Core.Resources
{
  using Core.Enterprise;

  public class SimulationNode
  {
    public SimulationNode(DayTime dayTime, Enterprise ent, Customer customer)
    {
      DayTime = dayTime;
      Enterprise = ent;
      Customer = customer;
    }

    public Customer Customer { get; }
    public DayTime DayTime { get; }
    public Enterprise Enterprise { get; }
  }
}