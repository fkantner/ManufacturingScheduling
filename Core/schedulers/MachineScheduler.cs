namespace Core.Schedulers
{
  using Core.Resources;
  using System.Collections.Generic;

  public interface IScheduleMachines
  {
      int ChooseNextWoId(ICustomQueue queue);
  }

  public class MachineScheduler : IScheduleMachines
  {
    public enum Schedule { DEFAULT=0 };

    private readonly Schedule _schedule;

    public MachineScheduler(Schedule schedule=(Schedule) 0)
    {
      _schedule = schedule;
    }

    public int ChooseNextWoId(ICustomQueue queue)
    {
      return _schedule switch
      {
        Schedule.DEFAULT => queue.FirstId().Value
      };
    }
  }
}