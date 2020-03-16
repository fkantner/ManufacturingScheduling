namespace Core.Plant
{
  using Core.Workcenters;
  using Core.Resources;
  using Core.Schedulers;

  public interface ITransportWork
  {
    string CurrentLocation { get; }
    int CargoNumber { get; }
    string Destination { get; }
    int TransportTime { get; }
    void Work(DayTime dayTime);
  }

  public class Transportation : ITransportWork
  {
    private IAcceptWorkorders _current_location;
    private IAcceptWorkorders _destination;
    private int _transport_time;
    private IWork _cargo;
    private IScheduleTransport _scheduler;

    public Transportation(IAcceptWorkorders start, IScheduleTransport ts)
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