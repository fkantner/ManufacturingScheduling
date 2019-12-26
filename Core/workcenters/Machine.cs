namespace Core.Workcenters
{
  using Core.Resources;
  using Core.Schedulers;
  using System.Collections.Generic;

  public class Machine
  {
    private List<Workorder> queue;
    private MachineScheduler scheduler;
    private readonly string TYPE;

    public Machine(MachineScheduler ms, string type)
    {
      queue = new List<Workorder>();
      scheduler = ms;
      TYPE = type;
    }

    public void Add(Workorder wc)
    {
      queue.Add(wc);
      return;
    }
  }
}
