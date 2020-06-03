namespace Core.Schedulers
{
  using Core.Enterprise;
  using Core.Resources;

  public enum EnterpriseSchedule { DEFAULT=0 };

  public interface IScheduleEnterprise
  {
    string SelectDestinationForExternalTransport(int woid);
  }

  public class EnterpriseScheduler : IScheduleEnterprise
  {
    private readonly IErp _erp;
    private readonly EnterpriseSchedule _schedule;

    public EnterpriseScheduler(IErp erp, EnterpriseSchedule schedule = (EnterpriseSchedule) 0)
    {
      _erp = erp;
      _schedule = schedule;
    }

    public string SelectDestinationForExternalTransport(int woid)
    {
      return _schedule switch
      {
        _ => GetFirstValidDestination(woid)
      };
    }

    private string GetFirstValidDestination(int woid)
    {
      IWork wo = _erp.Workorders[woid];
      string type = wo.CurrentOpType;
      foreach( var location in _erp.Locations)
      {
        var plant = location.Value;
        if(plant.CanWorkOnType(type))
        {
          return plant.Name;
        }
      }

      throw new System.ArgumentOutOfRangeException("No plant can work on type {0}", type);
    }
  }
}