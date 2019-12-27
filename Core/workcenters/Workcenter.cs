namespace Core.Workcenters
{
  using Resources;

  public class Workcenter
  {
    private Machine machine;
    private string name;
    public Workcenter(string name, Machine machine)
    {
      this.name = name;
      this.machine = machine;
    }

    public void Add(Workorder wc)
    {
      machine.Add(wc);
      return;
    }

    public override string ToString()
    {
      return "WC: " + name + "\n\tMachine: " + machine.ToString();
    }
  }
}