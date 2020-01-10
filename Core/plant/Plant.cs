namespace Core.Plant
{
  using System.Collections.Generic;
  using Core.Workcenters;
  //using Core.Resources;

  public class Plant
  {
    private readonly string _name;
    private IEnumerable<Workcenter> _workcenters;

    public Plant(string name, IEnumerable<Workcenter> workcenters)
    {
      _name = name;
      _workcenters = workcenters;
    }

    public string Name { get => _name; }
    public IEnumerable<Workcenter> Workcenters { get => _workcenters; }

    public void Work( DayTime dt )
    {
      foreach(Workcenter wc in _workcenters)
      {
        wc.Work(dt);
      }

      //TODO Add Transportation to Plant
      //TODO Add Shipping to Plant
      //TODO Add Receiving to Plant
      //TODO Add Dock to Plant
      //TODO Add Staging to Plant
      //TODO Add MES to Plant
    }
  }
}