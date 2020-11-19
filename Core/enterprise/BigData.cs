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
    private Dictionary<string, (DayTime, DayTime)> _workcenters;
    private int _schedule;
    private int _WC_counter;

    public BigData()
    {
      _workcenters = new Dictionary<string, (DayTime, DayTime)>();
      _schedule = Core.Configuration.BigDataSchedule;
      _WC_counter = 0;
    }

    public void AddWorkcenter(string wc)
    {
      _workcenters.Add(wc, GetDayTimeSet(_WC_counter));
      _WC_counter++;
    }

    public DayTime IsBreakdown(string workcenterName, DayTime dayTime)
    {
      var set = _workcenters[workcenterName];
      if(dayTime.GreaterThan(set.Item1) && dayTime.LessThan(set.Item2))
      {
        return null;
      }
      return dayTime;
    }

    private (DayTime, DayTime) GetDayTimeSet(int index)
    {
      //1440 mins / day
      var tuplelist = new List<(DayTime start, DayTime end)>
      {
        (new DayTime(0, 100), new DayTime(0, 200)),
        (new DayTime(0, 700), new DayTime(0, 750)),
        (new DayTime(1, 400), new DayTime(1, 600)),
        (new DayTime(1, 500), new DayTime(1, 520)),
        (new DayTime(2, 200), new DayTime(2, 230)),
        (new DayTime(2, 1000), new DayTime(2, 1050)),
        (new DayTime(3, 400), new DayTime(3, 1000)),
        (new DayTime(3, 200), new DayTime(3, 250)),
        (new DayTime(4, 1000), new DayTime(4, 1100)),
        (new DayTime(4, 600), new DayTime(4, 650)),
        (new DayTime(5, 100), new DayTime(5, 200)),
        (new DayTime(6, 150), new DayTime(6, 200)),
        (new DayTime(7, 100), new DayTime(7, 200))
      };

      return tuplelist[index % tuplelist.Count];
    }
  }
}