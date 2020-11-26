namespace Core.Enterprise
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Resources;

  public interface IHandleBigData
  {
    void AddWorkcenter(string wc);
    (Workorder.PoType, int)? GetNextOrder(int time);
    DayTime IsBreakdown(string workcenterName, DayTime dayTime);
    bool IsNonConformance(string workcenterName);
  }

  public class BigData : IHandleBigData
  {
    private Dictionary<string, (DayTime, DayTime)> _workcenterBreakdowns;
    private Dictionary<string, (int, int)> _workcenterNonconformance;
    private int _schedule;
    private int _WC_counter;
    private int _order_counter;

    public BigData()
    {
      _workcenterBreakdowns = new Dictionary<string, (DayTime, DayTime)>();
      _workcenterNonconformance = new Dictionary<string, (int, int)>();
      _schedule = Core.Configuration.BigDataSchedule;
      _WC_counter = 0;
      _order_counter = 0;
    }

    public void AddWorkcenter(string wc)
    {
      _workcenterBreakdowns.Add(wc, GetWorkcenterDayTimeSet(_WC_counter));
      int nonCom = _WC_counter * 10 + 10;
      _workcenterNonconformance.Add(wc, ( nonCom, nonCom ));
      _WC_counter++;
    }

    public (Workorder.PoType, int)? GetNextOrder(int time)
    {
      if (time % 60 == 0)
      {
        int day = time / Configuration.MinutesInDay;
        if (day < 6) day = day + 1;

        Workorder.PoType type = (Workorder.PoType) _order_counter;
        _order_counter = (_order_counter + 1) % 13;

        return (type, day);
      }
      return null;
    }

    public DayTime IsBreakdown(string workcenterName, DayTime dayTime)
    {
      var set = _workcenterBreakdowns[workcenterName];
      if(dayTime.GreaterThan(set.Item1) && dayTime.LessThan(set.Item2))
      {
        return null;
      }
      return dayTime;
    }

    public bool IsNonConformance(string workcenterName )
    {
      var set = _workcenterNonconformance[workcenterName];
      var orig = set.Item1;
      var next = set.Item2 - 1;
      
      if (next == 0)
      {
        next = orig;
      }

      _workcenterNonconformance[workcenterName] = (orig, next);

      return orig == next;
    }

    private (DayTime, DayTime) GetWorkcenterDayTimeSet(int index)
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