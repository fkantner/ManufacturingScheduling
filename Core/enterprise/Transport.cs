namespace Core.Enterprise
{
  using System.Collections.Generic;
  using System.Linq;
  using Core.Plant;
  using Core.Resources;

  public interface ITransportWorkBetweenPlants
  {
    string CurrentCargo { get; }
    string CurrentLocation { get; }
    void Work(DayTime dayTime);
  }

  public class Transport : ITransportWorkBetweenPlants
  {
// Properties
    public string CurrentCargo {
      get => string.Join(',', _cargo.Select(x => x.Wo.Id));
    }
    public string CurrentLocation { get => _current_location?.Name; }

// Constructor
    public Transport(IEnterprise company, Dictionary<DayTime, string> routes)
    {
      _current_location = null;
      _company = company;
      _routes = routes;
      _cargo = new List<Cargo>();
    }

// Pure Methods
    public Dictionary<int, string> Inventory()
    {
      Dictionary<int, string> answer = new Dictionary<int, string>();

      _cargo.ForEach(x => answer.Add(x.Wo.Id, x.Destination));
      
      return answer;
    }

// Impure Methods
    public void Work(DayTime dayTime)
    {
      if(!HasRouteStop(dayTime)) 
      { 
        if (!TimeToLeave(dayTime)) { return; }
        
        // Leave Plant
        _current_location = null; 
        
        // Deliver to customer
        List<Cargo> toCustomer = _cargo.Where(x => x.Destination == "customer").ToList();
        toCustomer.ForEach(x => _company.Customer.ReceiveProduct(x.Wo.ProductType, dayTime));
        toCustomer.ForEach(x => _cargo.Remove(x));
      
        return; 
      }

      var route = RouteStop(dayTime);
      
      _current_location = _company.Plants.FirstOrDefault(x => x.Name == route);
      if (_current_location == null) { return; } // Shouldn't hit this

      // Dropoff Shipments
      List<Cargo> deliveries = _cargo.Where( x => x.Destination == CurrentLocation).ToList();
      deliveries.ForEach(x => _current_location.Dock().ReceiveFromExternal(x.Wo));
      deliveries.ForEach(x => _cargo.Remove(x));

      // Pickup Shipments
      Dictionary<IWork, string> list = _current_location.ShipToOtherPlants();
      list.Keys.ToList().ForEach(wo => _cargo.Add(new Cargo(wo, list[wo])));
    }

// Private
    private readonly List<Cargo> _cargo;
    private readonly IEnterprise _company;
    private IPlant _current_location;
    private readonly Dictionary<DayTime, string> _routes;
    
    private bool HasRouteStop(DayTime dayTime)
    {
      return _routes.Keys.Where(x => x.Equals(dayTime)).Any();
    }
    
    private string RouteStop(DayTime dayTime)
    {
      DayTime key = _routes.Keys.FirstOrDefault(x => x.Equals(dayTime));
      
      if(key == null) return "No Stop";
      return _routes[key];
    }

    private bool TimeToLeave(DayTime dayTime)
    {
      return _routes.Keys.Where(x => x.CreateTimestamp(5).Equals(dayTime)).Any();
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