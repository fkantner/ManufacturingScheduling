namespace Core.Schedulers
{
  using Core.Plant;
  using Core.Resources;

  public interface ISchedulePlants
  {
    int ValidateWoForMachines(int woid, string location);
    int ValidateWoForTransport(int woid, string location);
    string ValidateDestinationForTransport(int? woid, string location, string destination);
  }

  public class PlantScheduler : ISchedulePlants
  {
    private readonly IMes _mes;
    public enum Schedule { DEFAULT=0 };
    private readonly Schedule _schedule;
    public PlantScheduler(IMes mes, Schedule schedule=(Schedule) 0)
    {
      _mes = mes;
      _schedule = schedule;
    }
    // TODO - Implement PlantScheduler
    // TODO - Connect Plant Scheduler to Machine Scheduler

    public int ValidateWoForMachines(int woid, string location)
    {
      return _schedule switch
      {
        Schedule.DEFAULT => woid
      };
    }

    public int ValidateWoForTransport(int woid, string location)
    {
      return _schedule switch
      {
        Schedule.DEFAULT => woid
      };
    }

    public string ValidateDestinationForTransport(int? woid, string location, string destination)
    {
      return _schedule switch
      {
        Schedule.DEFAULT => destination
      };
    }
  }
}