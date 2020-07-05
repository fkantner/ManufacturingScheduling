namespace Core.Schedulers
{
  using Core.Plant;
  using Core.Resources;

  public enum PlantSchedule { DEFAULT=0 };

  public interface ISchedulePlants
  {
    string ValidateDestinationForTransport(int? woid, string location, string destination);
    int ValidateWoForMachines(int woid, string location);
    int? ValidateWoForTransport(int? woid, string location);
  }

  public class PlantScheduler : ISchedulePlants
  {
    public PlantScheduler(IMes mes)
    {
      _mes = mes;
      _schedule = (PlantSchedule) Configuration.PlantSchedule;
    }

    public string ValidateDestinationForTransport(int? woid, string location, string destination)
    {
      return _schedule switch
      {
        _ => destination
      };
    }

    public int ValidateWoForMachines(int woid, string location)
    {
      return _schedule switch
      {
        _ => woid
      };
    }

    public int? ValidateWoForTransport(int? woid, string location)
    {
      return _schedule switch
      {
        _ => UseWoIfInMes(woid, location)
      };
    }

    private readonly IMes _mes;
    private readonly PlantSchedule _schedule;

    private int? UseWoIfInMes(int? woid, string location)
    {
      if(!woid.HasValue) { return woid; }
      if(_mes.Workorders.ContainsKey(woid.Value)) { return woid; }
      
      // Might not have made it to MES, yet.
      foreach( IWork wo in _mes.Locations[location].OutputBuffer)
      {
        if(_mes.Workorders.ContainsKey(wo.Id)) { return wo.Id; }
      }

      return null;
    }
  }
}