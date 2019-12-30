namespace Core
{
  public class DayTime
  {
    enum Days {Sun, Mon, Tue, Wed, Thu, Fri, Sat};
    private int _day;
    private int _time;    
    const int MinutesInDay = 24*60;

    // CONSTRUCTORS //
    public DayTime()
    {
      _day = (int) DayTime.Days.Wed;
      _time = 0;
    }

    public DayTime(int setDay, int setTime)
    {
      _day = setDay;
      _time = setTime;
    }

    // PROPERTIES //
    public int Day { get => _day; }
    public int Time { get => _time; }

    // PUBLIC METHODS //
    public DayTime CreateTimestamp(int increment)
    {
      DayTime timestamp = new DayTime(Day, Time);

      while(increment > 0)
      {
        timestamp.Next();
        increment = increment - 1;
      }

      return timestamp;
    }

    public bool Equals(DayTime other)
    {
      return other.Day == this.Day && other.Time == this.Time;
    }

    public bool LessThan(DayTime other)
    {
      return this.Day < other.Day || (this.Day == other.Day && this.Time < other.Time);
    }

    public void Next()
    {
      
      _time += 1;

      if (_time >= MinutesInDay)
      {
        _day += 1;
        _time = 0;

        if (_day > (int)Days.Sat)
        {
          _day = (int) Days.Sun;
        }
      }
    }

    // PRIVATE METHODS //

    private string GetTime()
    {
      int hour = _time / 60;
      int minutes = _time % 60;
      return hour + ":" + minutes;
    }

    private string GetDay()
    {
      switch (_day)
      {
        case 0:
          return "Sun";
        case 1:
          return "Mon";
        case 2:
          return "Tue";
        case 3:
          return "Wed";
        case 4:
          return "Thu";
        case 5:
          return "Fri";
        case 6:
          return "Sat";
      }
      return "Error";
    }

  }
}