namespace Core
{
  public class DayTime
  {
    public enum Days {Sun, Mon, Tue, Wed, Thu, Fri, Sat};

    // CONSTRUCTORS //
    public DayTime()
    {
      Day = (int) Days.Sun;
      Time = 0;
    }

    public DayTime(int setDay, int setTime)
    {
      Day = setDay;
      Time = setTime;
    }

    // PROPERTIES //
    public int Day { get; private set; }
    public int Time { get; private set; }

    // PUBLIC METHODS //
    public DayTime CreateTimestamp(int increment)
    {
      DayTime timestamp = new DayTime(Day, Time);

      while(increment > 0)
      {
        timestamp.Next();
        increment--;
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
      Time++;

      if (Time >= Configuration.MinutesInDay)
      {
        Day++;
        Time = 0;

        if (Day > (int) Days.Sat)
        {
          Day = (int) Days.Sun;
        }
      }
    }

    // PRIVATE METHODS //
/* May ReAdd later
    private string GetTime()
    {
      int hour = Time / 60;
      int minutes = Time % 60;
      return hour + ":" + minutes;
    }
*/
/*
    private string GetDay()
    {
      switch (Day)
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
*/
  }
}
