namespace Core.Enterprise
{
  using Core.Plant;
  using Core.Resources;
  using System.Collections.Generic;
  using System.Linq;

  public interface ITransportWorkBetweenPlants
  {
    string CurrentCargo { get; }
    string CurrentLocation { get; }
    void Work(DayTime dayTime);
  }

  public class Transport : ITransportWorkBetweenPlants
  {
    private IPlant _current_location;
    public string CurrentLocation { get => _current_location?.Name; }
    public string CurrentCargo {
      get => string.Join(',', _cargo.Select(x => x.Wo.Id));
    }

    private readonly IEnterprise _company;
    private readonly Dictionary<DayTime, string> _routes;
    private readonly List<Cargo> _cargo;

    public Transport(IEnterprise company, Dictionary<DayTime, string> routes)
    {
      _current_location = null;
      _company = company;
      _routes = routes;
      _cargo = new List<Cargo>();
    }

    public Dictionary<int, string> Inventory()
    {
      Dictionary<int, string> answer = new Dictionary<int, string>();

      foreach(Cargo c in _cargo)
      {
        answer.Add(c.Wo.Id, c.Destination);
      }
      
      return answer;
    }

    public void Work(DayTime dayTime)
    {
      if(!HasRouteStop(dayTime)) { 
        if(TimeToLeave(dayTime)){ _current_location = null; }
        return; 
      }
      var route = RouteStop(dayTime);
      
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
      List<Cargo> deliveries = new List<Cargo>();
      foreach(Cargo item in _cargo)
      {
        if(item.Destination == CurrentLocation)
        {
          _current_location.Dock().ReceiveFromExternal(item.Wo);
          deliveries.Add(item);
        }
      }
      
      foreach(Cargo item in deliveries)
      {
        _cargo.Remove(item);
      }

      // Pickup Shipments
      Dictionary<IWork, string> list = _current_location.ShipToOtherPlants();

      foreach(IWork wo in list.Keys)
      {
        _cargo.Add(new Cargo(wo, list[wo]));
      }
    }

    private bool HasRouteStop(DayTime dayTime)
    {
      foreach( var time in _routes.Keys)
      {
        if(time.Equals(dayTime)) { return true; }
      }
      return false;
    }

    private string RouteStop(DayTime dayTime)
    {
      DayTime key = null;
      foreach( var time in _routes.Keys)
      {
        if(time.Equals(dayTime)) 
        { 
          key = time; 
          break;
        }
      }
      if(key == null) return "No Stop";
      return _routes[key];
    }

    private bool TimeToLeave(DayTime dayTime)
    {
      foreach( var time in _routes.Keys )
      {
        if(time.CreateTimestamp(5).Equals(dayTime) ) { return true; }
      }
      return false;
    }

    private class Cargo
    {
      public IWork Wo { get; }
      public string Destination { get; }

      public Cargo(IWork work, string destination)
      {
        Wo = work;
        Destination = destination;
      }
    }
  }
}