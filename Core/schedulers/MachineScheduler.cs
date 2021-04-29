namespace Core.Schedulers
{
  using System.Linq;
  using System.Collections.Generic;
  using Core.Plant;
  using Core.Resources;
  using Core.Schedulers;
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
      var increment = from wo in queue
        where ( wo.CurrentOpType == last )
        select wo.Id;

      //Increasing the value for those with the last set.
      ratings.Where(x => passedIds.Contains(x.Object)).ToList().ForEach(x => x.Value += Configuration.MachineRatingIncreaseForSameTypeAsLastType);
      
      ratings.Sort();
      
      return ratings.First().Object;
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

  }
}