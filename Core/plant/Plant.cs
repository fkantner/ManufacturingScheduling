namespace Core.Plant
{
  using System.Collections.Generic;
  using Core.Workcenters;
  //using Core.Resources;

  public class Plant
  {
    private readonly string _name;
    private IEnumerable<IAcceptWorkorders> _workcenters;
    private Transportation _transportation;

    public Plant(string name, IEnumerable<IAcceptWorkorders> workcenters)
    {
      _name = name;
      _workcenters = workcenters;
    }

    public string Name { get => _name; }
    public IEnumerable<IAcceptWorkorders> Workcenters { get => _workcenters; }
    public Transportation InternalTransportation { get => _transportation; set => _transportation = value; }

    public void Work( DayTime dt )
    {
      foreach(IAcceptWorkorders wc in _workcenters)
      {
        wc.Work(dt);
      }

      _transportation.Work(dt);

      //TODO Add Transportation to Plant
      //TODO Add Shipping to Plant
      //TODO Add Receiving to Plant
      //TODO Add Dock to Plant
      //TODO Add Staging to Plant
      //TODO Add MES to Plant
    }
  }
}