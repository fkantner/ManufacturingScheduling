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
    private IPlant _plant;
    private Machine _machine;

    public enum Schedule { DEFAULT=0, MATCH_LAST=1, BASIC_SCHEDULE=2 };

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
        Schedule.BASIC_SCHEDULE => GetBasicSchedule(queue),
        _ => queue.FirstId().Value
      };

      return proposed;
    }

    private int GetBasicSchedule<T>(T queue) where T : ICustomQueue, IEnumerable<IWork>
    {
      IEnumerable<int> passedIds = queue.Select(x => x.Id);
      List<Rating<int>> ratings = _plant.PlantScheduler.GetWorkorderRatings();
      ratings = ratings.Where(x => passedIds.Contains(x.Object)).ToList();

      Core.Resources.Op.OpTypes last = _machine.LastType;
      var wosWithSameAsLastType = from wo in queue
        where ( wo.CurrentOpType == last )
        select wo.Id;
      ratings.Where(x => wosWithSameAsLastType.Contains(x.Object)).ToList().ForEach(x => x.Value += Configuration.MachineOpTypeVariable);
      
      ratings.Where(x => !wosWithSameAsLastType.Contains(x.Object)).ToList().ForEach(x => x.Value += GetWaitTime(x.Object) * Configuration.MachineWaitTimeVariable);

      int maxValue = ratings.Max(x => x.Value);
      return ratings.First(x => x.Value == maxValue).Object;
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

    private int GetWaitTime(int woid)
    {
      var wo = _plant.Mes.Workorders.First(x => x.Key == woid).Value;

      if(wo.CurrentOpType == _machine.LastType)
      {
        return wo.CurrentOpSetupTime;
      }

      return 0;
    }

  }
}