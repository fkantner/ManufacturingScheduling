namespace Core.Schedulers
{
  using Core.Plant;
  using Core.Resources;

  public interface IScheduleMachines
  {
    void AddPlant(Plant plant);
    int ChooseNextWoId(string machineName, ICustomQueue queue);
  }

  public class MachineScheduler : IScheduleMachines
  {
    public enum Schedule { DEFAULT=0 };

    private readonly Schedule _schedule;
    private Plant _plant;

    public MachineScheduler(Schedule schedule=(Schedule) 0)
    {
      _plant = null;
      _schedule = schedule;
    }

    public void AddPlant(Plant plant)
    {
      if (_plant == null)
      {
        _plant = plant;
      }
    }

    public int ChooseNextWoId(string machineName, ICustomQueue queue)
    {
      int proposed = _schedule switch
      {
        Schedule.DEFAULT => queue.FirstId().Value
      };

      return _plant.PlantScheduler.ValidateWoForMachines(proposed, machineName);
    }
  }
}