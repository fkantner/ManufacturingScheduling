namespace Core.Enterprise
{
  using Core;
  using Plant;
  using System.Collections.Generic;

  public class Enterprise
  {
    public Enterprise(DayTime dayTime, IEnumerable<IPlant> plants)
    {
      DayTime = dayTime;
      Plants = plants;
      Erp = (IErp) new Erp(DayTime, Plants);
    }

    public DayTime DayTime { get; }
    public IEnumerable<IPlant> Plants { get; }
    public IErp Erp { get; }

    public void Work(DayTime dayTime)
    {
      foreach(IPlant plant in Plants)
      {
        plant.Work(dayTime);
      }

      Erp.Work(dayTime);
    }
  }
}