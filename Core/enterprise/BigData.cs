namespace Core.Enterprise
{
  using System.Collections.Generic;

  public interface IHandleBigData
  {
    void AddWorkcenter(string wc);
    DayTime IsBreakdown(string workcenterName, DayTime dayTime);
  }

  public class BigData : IHandleBigData
  {
    private Dictionary<string, int> _workcenters;
    private int _schedule;

    public BigData()
    {
      _workcenters = new Dictionary<string, int>();
      _schedule = Core.Configuration.BigDataSchedule;
    }

    public void AddWorkcenter(string wc)
    {
      _workcenters.Add(wc, _schedule);
    }

    public DayTime IsBreakdown(string workcenterName, DayTime dayTime)
    {
      return dayTime;
    }
  }
}