namespace Core.Plant
{
  using System.Collections.Generic;
  using Core.Schedulers;
  using Core.Workcenters;
  public interface IPlant
  {
    IMes Mes { get; }
    string Name { get; }
    ISchedulePlants PlantScheduler { get; }
    IEnumerable<IAcceptWorkorders> Workcenters { get; }
    ITransportWork InternalTransportation { get; set; }
    void Work(DayTime dt);
  }

  public class Plant : IPlant
  {
    public Plant(string name, IEnumerable<IAcceptWorkorders> workcenters)
    {
      Name = name;
      Workcenters = workcenters;

      Dictionary<string, IAcceptWorkorders> locations = new Dictionary<string, IAcceptWorkorders>();
      foreach(IAcceptWorkorders wc in Workcenters)
      {
        locations.Add(wc.Name, wc);
        wc.AddPlant(this);
      }
      Mes = (IMes) new Mes("MES", locations);

      PlantScheduler = (ISchedulePlants) new PlantScheduler(Mes, PlantSchedule.DEFAULT);
    }

    public IMes Mes { get; }
    public string Name { get; }
    public ISchedulePlants PlantScheduler { get; }
    public IEnumerable<IAcceptWorkorders> Workcenters { get; }
    public ITransportWork InternalTransportation { get; set; }

    public void Work( DayTime dt )
    {
      foreach(IAcceptWorkorders wc in Workcenters)
      {
        wc.Work(dt);
      }

      InternalTransportation.Work(dt);

      Mes.Work(dt);
    }
  }
}