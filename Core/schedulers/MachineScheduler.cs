namespace Core.Schedulers
{
  using System.Linq;
  using System.Collections.Generic;
  using Core.Plant;
  using Core.Resources;
  using Core.Workcenters;

  public interface IScheduleMachines
  {
    void AddPlant(IPlant plant);
    int ChooseNextWoId<T>(string machineName, T queue) where T : ICustomQueue, IEnumerable<IWork>;
  }

  public class MachineScheduler : IScheduleMachines
  {
    public enum Schedule { DEFAULT=0, MATCH_LAST=1 };

    public MachineScheduler(Machine toBeScheduled)
    {
      _plant = null;
      _machine = toBeScheduled;
    }

    public void AddPlant(IPlant plant)
    {
      if(_plant != null) { return; }
      
      _plant = plant;
    }

    public int ChooseNextWoId<T>(string machineName, T queue) where T : ICustomQueue, IEnumerable<IWork>
    {
      Schedule schedule = (Schedule) Configuration.Instance.MachineSchedule;
      int proposed = schedule switch
      {
        Schedule.DEFAULT => queue.FirstId().Value,
        Schedule.MATCH_LAST => GetIdToMatchLast(queue),
        _ => queue.FirstId().Value
      };

      return _plant.PlantScheduler.ValidateWoForMachines(proposed, machineName);
    }

    private int GetIdToMatchLast<T>(T queue) where T : ICustomQueue, IEnumerable<IWork>
    {
      Core.Resources.Op.OpTypes last = _machine.LastType;
      var set = from wo in queue
        where ( wo.CurrentOpType == last )
        select wo;

      if(!set.Any()) { return queue.FirstId().Value; }
      
      return set.First().Id;
    }

    private IPlant _plant;
    private Machine _machine;
  }
}