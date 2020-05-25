namespace Core.Enterprise
{
  using Plant;
  using System.Collections.Generic;

  public interface IErp
  {
    void Work(DayTime dayTime);
  }

  public class Erp : IErp
  {
    public Erp(DayTime dayTime, IEnumerable<IPlant> plants)
    {
      DayTime = dayTime;
      Plants = plants;
    }

    public DayTime DayTime { get; }
    public IEnumerable<IPlant> Plants { get; }

    public void Work(DayTime dayTime)
    {
      // Do Something
    }
  }
}