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
      List<Rating<int>> ratings = _erp.DueDates.Select(x => new Rating<int>(x.Key, (6-x.Value.Day)* Configuration.EnterpriseDueDateVariable)).ToList();
      ratings.ForEach(x => x.Value += GetJobValue(x.Object) * Configuration.EnterpriseTravelVariable);
      
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

    private int GetJobValue(int woid)
    {
      DayTime due = _erp.DueDates.First(x => x.Key == woid).Value;
      int dueValue = 6 - due.Day * Configuration.EnterpriseDueDateVariable;

      Resources.Virtual.VirtualWorkorder wo = _erp.Workorders.First(x => x.Key == woid).Value;

      int moves = 0;
      for(int i = wo.CurrentOpIndex; i<wo.Operations.Count; i++)
      {
        if( i == 0 ){ continue; }
        var optypeN = wo.Operations[i].Type;
        var optypeN_1 = wo.Operations[i-1].Type;

        var plantsA = _erp.Locations.Values.Where(x => !x.CanWorkOnType(optypeN));
        var plantsB = _erp.Locations.Values.Where(x => x.CanWorkOnType(optypeN_1));

        if(plantsA.Intersect(plantsB).Count() > 0){ moves++; }
      }
      return moves;
    }

    private string GetFirstValidDestination(int woid, Op currentOp)
    {
      Op.OpTypes type = currentOp.Type;
      
      return _erp.Locations.First(x => x.Value.CanWorkOnType(type)).Value.Name;
    }

    private readonly IErp _erp;
  }
}