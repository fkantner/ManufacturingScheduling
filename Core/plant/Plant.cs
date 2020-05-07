namespace Core.Plant
{
  using System.Collections.Generic;
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
      }
      Mes = (IMes) new Mes("MES", locations);
    }

    public IMes Mes { get; }
    public string Name { get; }
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

      //TODO Add Shipping to Plant
      //TODO Add Receiving to Plant
      //TODO Add Dock to Plant
      //TODO Add Staging to Plant
    }
  }
}