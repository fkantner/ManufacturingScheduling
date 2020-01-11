namespace Core.Plant
{
  using Core.Workcenters;
  using Core.Resources;
  using Core.Schedulers;

  public class Transportation
  {
    private IAcceptWorkorders _current_location;
    private IAcceptWorkorders _destination;
    private int _transport_time;
    private Workorder _cargo;
    private TransportationScheduler _scheduler;

    private const int _default_transport_time = 5;

    public Transportation(IAcceptWorkorders start, TransportationScheduler ts)
    {
      _current_location = start;
      _destination = null;
      _transport_time = 0;
      _cargo = null;
      _scheduler = ts;
    }

    public string CurrentLocation { get => _current_location.Name; }
    public int CargoNumber { get => _cargo == null ? 0 : _cargo.Id; }
    public string Destination { get => _destination == null ? "None" : _destination.Name; }
    public int TransportTime { get => _transport_time; }
    
    public void Work(DayTime dayTime)
    {
      if(_cargo == null && _transport_time == 0) // Pickup cargo.
      {
        if(_destination != null){
          _current_location = _destination;
          _destination = null;
        }
        _scheduler.ChooseNextCargo(_current_location);
        _cargo = _scheduler.GetCargo(_current_location);
        _destination = _scheduler.Destination;
        _transport_time = _scheduler.TransportTime;
      }
      else if (_transport_time == 0) // Dropping off Cargo
      {
        _current_location = _destination;
        _current_location.AddToQueue(_cargo);
        _cargo = null;
        _destination = null;
      } 
      else //In Route
      {
        _transport_time -= 1;
      }

    }

  }
}