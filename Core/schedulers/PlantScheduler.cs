namespace Core.Schedulers
{
  using System.Collections.Generic;
  using Core.Plant;
  using Core.Resources;
  using System.Linq;

  public enum PlantSchedule { DEFAULT=0 };

  public interface ISchedulePlants
  {
    string ValidateDestinationForTransport(int? woid, string location, string destination);
    int ValidateWoForMachines(int woid, string location);
    int? ValidateWoForTransport(int? woid, string location);
    List<Rating<int>> GetWorkorderRatings();
  }

  public class PlantScheduler : ISchedulePlants
  {
    public PlantScheduler(IPlant plant)
    {
      _plant = plant;
      _mes = _plant.Mes;
      _schedule = (PlantSchedule) Configuration.Instance.PlantSchedule;
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

    public List<Rating<int>> GetWorkorderRatings()
    {
      var woRatings = _plant.GetEnterprise().Scheduler.GetWorkorderRatings();
      //Filtering
      var ids = _mes.Workorders.Keys;
      var filteredRatings = woRatings.Where(x => ids.Contains(x.Object)).ToList();

      filteredRatings.ForEach(x => x.Value += GetRemainingOpTime(x.Object) * Configuration.PlantOperationTimeVariable);
      filteredRatings.ForEach(x => x.Value += GetRemainingOpCount(x.Object) * Configuration.PlantOperationCountVariable);

      return filteredRatings;
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
    private readonly IPlant _plant;

    private int GetRemainingOpTime(int woid)
    {
      var wo = GetVirtualWorkorder(woid);
      int time = 0;
      for(int i = wo.CurrentOpIndex; i < wo.Operations.Count; i++)
      {
        time += wo.Operations[i].EstTimeToComplete;
      }

      return time;
    }

    private int GetRemainingOpCount(int woid)
    {
      var wo = GetVirtualWorkorder(woid);
      return wo.Operations.Count - wo.CurrentOpIndex;
    }

    private Resources.Virtual.VirtualWorkorder GetVirtualWorkorder(int woid)
    {
      return (Resources.Virtual.VirtualWorkorder) _mes.Workorders.First(x => x.Key == woid).Value;
    }

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