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
      var woRatings = _plant.Enterprise.Scheduler.GetWorkorderRatings();

      string minKey = "";
      int minValue = 0;
      var wcs =_mes.LocationInventories.Keys;
      foreach(string wc in wcs)
      {
        var vals = _mes.LocationInventories[wc];
        var total = vals.Select(x => x.CurrentOpEstTimeToComplete).Sum();
        if (string.IsNullOrEmpty(minKey) || total < minValue) 
        {
          minKey = wc;
          minValue = total;
        }
      }

      var types = _mes.Locations[minKey].ListOfValidTypes();

      var ids = _mes.Workorders.Where(x => types.Contains(x.Value.CurrentOpType)).Select(x => x.Key);

      woRatings.Where(x => ids.Contains(x.Object)).ToList().ForEach(x => x.Value += Configuration.PlantRatingIncreaseForRemainingInMachines);

      return woRatings;
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