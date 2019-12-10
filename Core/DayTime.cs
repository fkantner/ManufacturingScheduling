namespace Core
{
  public class DayTime
  {
    enum Days {Sun, Mon, Tue, Wed, Thu, Fri, Sat};
    private int day;
    private int time;

    const int MinutesInDay = 24*60;

    public DayTime()
    {
      day = (int) DayTime.Days.Wed;
      time = 0;
    }

    public int Day() { return day; }

    public int Time() { return time; }


    public void Next()
    {
      time += 1;

      if (time >= MinutesInDay)
      {
        day += 1;
        time = 0;

        if (day > (int)Days.Sat)
        {
          day = (int) Days.Sun;
        }
      }
    }

    public override string ToString()
    {
      return "Day: " + GetDay() + " Time: " + GetTime();
    }

    private string GetTime()
    {
      int hour = time / 60;
      int minutes = time % 60;
      return hour + ":" + minutes;
    }

    private string GetDay()
    {
      switch (day)
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