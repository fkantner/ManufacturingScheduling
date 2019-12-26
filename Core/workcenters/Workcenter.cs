namespace Core.Workcenters
{
  using Resources;

  public class Workcenter
  {
    private Machine machine;
    public Workcenter(Machine machine)
    {
      this.machine = machine;
    }

    public void Add(Workorder wc)
    {
      machine.Add(wc);
      return;
    }
  }
}