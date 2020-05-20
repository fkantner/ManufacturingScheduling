namespace Core.Plant
{
  using System.Collections.Generic;
  using Core.Schedulers;
  using Core.Workcenters;

  public class Plant
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

      PlantScheduler = new PlantScheduler(Mes, PlantScheduler.Schedule.DEFAULT);
    }

    public IMes Mes { get; }
    public string Name { get; }
    public PlantScheduler PlantScheduler { get; }
    public IEnumerable<IAcceptWorkorders> Workcenters { get; }
    public Transportation InternalTransportation { get; set; }

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