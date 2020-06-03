namespace Core.Enterprise
{
  using Core.Plant;
  using Core.Resources;
  using System.Collections.Generic;

  public interface ITransportWorkBetweenPlants
  {
    string CurrentLocation { get; }
    void Work(DayTime dayTime);
  }

  public class Transport : ITransportWorkBetweenPlants
  {
    private IPlant _current_location;
    public string CurrentLocation { get => _current_location?.Name; }

    private readonly Enterprise _company;
    private readonly Dictionary<DayTime, string> _routes;
    private readonly List<Cargo> _cargo;

    public Transport(Enterprise company, Dictionary<DayTime, string> routes)
    {
      _current_location = null;
      _company = company;
      _routes = routes;
      _cargo = new List<Cargo>();
    }

    public void Work(DayTime dayTime)
    {
      if (!_routes.ContainsKey(dayTime)) { return; }
      var route = _routes[dayTime];

      //Find current location;
      foreach( IPlant p in _company.Plants)
      {
        if(p.Name == route)
        {
          _current_location = p;
          break;
        }
      }

      if (_current_location == null) { return; }
      // Dropoff Shipments
      foreach(Cargo item in _cargo)
      {
        if(item.Destination == CurrentLocation)
        {
          _current_location.Dock().ReceiveFromExternal(item.Wo);
          _cargo.Remove(item);
        }
      }

      // Pickup Shipments
      Dictionary<IWork, string> list = _current_location.ShipToOtherPlants();

      foreach(IWork wo in list.Keys)
      {
        _cargo.Add(new Cargo(wo, list[wo], dayTime));
      }
    }

    private class Cargo
    {
      public IWork Wo { get; }
      public string Destination { get; }
      public DayTime EarliestArrivalTime { get; }

      public Cargo(IWork work, string destination, DayTime now)
      {
        Wo = work;
        Destination = destination;
        EarliestArrivalTime = now.CreateTimestamp(30); // Adds 30 minutes of travel time.
      }
    }
  }
}