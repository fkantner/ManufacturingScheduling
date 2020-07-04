namespace Core.Schedulers
{
  using System.Linq;
  using Core.Enterprise;
  using Core.Resources;

  public enum EnterpriseSchedule { DEFAULT=0 };

  public interface IScheduleEnterprise
  {
    string SelectDestinationForExternalTransport(int woid);
  }

  public class EnterpriseScheduler : IScheduleEnterprise
  {
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
      string type = _erp.Workorders[woid].CurrentOpType;
      
      if (type == "shippingOp") { return "customer"; }

      return _erp.Locations.First(x => x.Value.CanWorkOnType(type)).Value.Name;
    }

    private readonly IErp _erp;
    private readonly EnterpriseSchedule _schedule;
  }
}