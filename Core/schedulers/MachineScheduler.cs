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

    public MachineScheduler()
    {
      _plant = null;
    }

    public void AddPlant(Plant plant)
    {
      if(_plant != null) { return; }
      
      _plant = plant;
    }

    public int ChooseNextWoId(string machineName, ICustomQueue queue)
    {
      Schedule schedule = (Schedule) Configuration.MachineSchedule;
      int proposed = schedule switch
      {
        _ => queue.FirstId().Value
      };

      return _plant.PlantScheduler.ValidateWoForMachines(proposed, machineName);
    }

    private Plant _plant;
  }
}