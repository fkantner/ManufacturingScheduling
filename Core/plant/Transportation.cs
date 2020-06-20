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
    private IWork _cargo;
    private readonly IScheduleTransport _scheduler;
    private readonly IMes _mes;

    public Transportation(IAcceptWorkorders start, IScheduleTransport ts)
    {
      _current_location = start;
      _destination = null;
      TransportTime = 0;
      _cargo = null;
      _scheduler = ts;
      _mes = ts.Mes;
    }

    public string CurrentLocation { get => _current_location.Name; }
    public int CargoNumber { get => _cargo?.Id ?? 0; }
    public string Destination { get => _destination?.Name ?? "None"; }
    public int TransportTime { get; private set; }

    public void Work(DayTime dayTime)
    {
      if(IsAtLocationWithoutCargo()) // Pickup cargo.
      {
        // Adding this check in case sitting and waiting for instruction
        if(HasADestination()){
          Arrive();
        }
        UpdateSelfFromScheduler();
      }
      else if (IsAtLocation()) // Dropping off Cargo
      {
        if(HasADestination()){
          Arrive();
        }
        DropOffCargo();
      }
      else //In Route
      {
        Transport();
      }
    }

    private void Arrive()
    {
      if(_destination != null)
      {
        _current_location = _destination;
      }
      _destination = null;
    }

    private void DropOffCargo()
    {
      if (_cargo == null) { return; }
      _current_location.AddToQueue(_cargo);
      _cargo = null;
    }

    private bool HasADestination()
    {
      return _destination != null;
    }

    private bool IsAtLocation()
    {
      return TransportTime == 0;
    }

    private bool IsAtLocationWithoutCargo()
    {
      return _cargo == null && TransportTime == 0;
    }

    private void Transport()
    {
      TransportTime--;
    }

    private void UpdateSelfFromScheduler()
    {
      _scheduler.ChooseNextCargo(_current_location);
      int? cargoId = _scheduler.GetCargoID();
      if(cargoId.HasValue)
      {
        _cargo = _current_location.OutputBuffer.Remove(cargoId.Value);
        _mes.StartTransit(cargoId.Value, _current_location.Name);
      }

      _destination = _scheduler.Destination;
      TransportTime = _scheduler.TransportTime;
    }
  }
}