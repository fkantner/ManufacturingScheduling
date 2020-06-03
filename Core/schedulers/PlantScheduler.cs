namespace Core.Schedulers
{
  using Core.Plant;
  using Core.Resources;

  public enum PlantSchedule { DEFAULT=0 };

  public interface ISchedulePlants
  {
    int ValidateWoForMachines(int woid, string location);
    int ValidateWoForTransport(int woid, string location);
    string ValidateDestinationForTransport(int? woid, string location, string destination);
  }

  public class PlantScheduler : ISchedulePlants
  {
    private readonly IMes _mes;
    private readonly PlantSchedule _schedule;
    public PlantScheduler(IMes mes, PlantSchedule schedule=(PlantSchedule) 0)
    {
      _mes = mes;
      _schedule = schedule;
    }

    public int ValidateWoForMachines(int woid, string location)
    {
      return _schedule switch
      {
        _ => woid
      };
    }

    public int ValidateWoForTransport(int woid, string location)
    {
      return _schedule switch
      {
        _ => woid
      };
    }

    public string ValidateDestinationForTransport(int? woid, string location, string destination)
    {
      return _schedule switch
      {
        _ => destination
      };
    }
  }
}