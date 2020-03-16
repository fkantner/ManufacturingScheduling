namespace Core.Schedulers
{
  using Core.Resources;
  using System.Collections.Generic;

  public interface IScheduleMachines
  {
      void Sort(Queue<IWork> queue);
  }

  public class MachineScheduler : IScheduleMachines
  {
    // TODO - Implement MachineScheduler

    public void Sort(Queue<IWork> queue)
    {
      // TODO - Implement Machine Scheduler Sort Algorithm
      return;
    }
  }
}