namespace Core.Schedulers
{
  using System.Collections.Generic;
  using System.Linq;
  using Core.Enterprise;
  using Core.Resources;

  public enum EnterpriseSchedule { DEFAULT=0 };

  public interface IScheduleEnterprise
  {
    string SelectDestinationForExternalTransport(int woid, Op currentOp);
    List<Rating<int>> GetWorkorderRatings();
  }

  public class EnterpriseScheduler : IScheduleEnterprise
  {
    public EnterpriseScheduler(IErp erp)
    {
      _erp = erp;
    }

    public List<Rating<int>> GetWorkorderRatings()
    {
      List<Rating<int>> ratings = _erp.DueDates.Select(x => new Rating<int>(x.Key, (6-x.Value.Day)*10)).ToList();
      
      return ratings;
    }

    public string SelectDestinationForExternalTransport(int woid, Op currentOp)
    {
      if(currentOp.Type == Op.OpTypes.ShippingOp) { return "customer"; }

      EnterpriseSchedule schedule = (EnterpriseSchedule) Configuration.Instance.EnterpriseSchedule;
      return schedule switch
      {
        _ => GetFirstValidDestination(woid, currentOp)
      };
    }

    private string GetFirstValidDestination(int woid, Op currentOp)
    {
      Op.OpTypes type = currentOp.Type;
      
      return _erp.Locations.First(x => x.Value.CanWorkOnType(type)).Value.Name;
    }

    private readonly IErp _erp;
  }
}