namespace Core.Schedulers
{
  using System.Linq;
  using Core.Enterprise;
  using Core.Resources;

  public enum EnterpriseSchedule { DEFAULT=0 };

  public interface IScheduleEnterprise
  {
    string SelectDestinationForExternalTransport(int woid, Op currentOp);
  }

  public class EnterpriseScheduler : IScheduleEnterprise
  {
    public EnterpriseScheduler(IErp erp)
    {
      _erp = erp;
    }

    public string SelectDestinationForExternalTransport(int woid, Op currentOp)
    {
      if(currentOp.Type == Op.OpTypes.ShippingOp) { return "customer"; }

      EnterpriseSchedule schedule = (EnterpriseSchedule) Configuration.EnterpriseSchedule;
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