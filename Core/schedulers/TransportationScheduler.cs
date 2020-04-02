namespace Core.Schedulers
{
  using Core.Resources;
  using Core.Workcenters;
  using Core.Plant;
  using System.Linq;

  public interface IScheduleTransport
  {
    public void ChooseNextCargo(IAcceptWorkorders current_location);
    public IWork GetCargo(IAcceptWorkorders current_location);
    public IAcceptWorkorders Destination { get; }
    public int TransportTime { get; }
  }

  public class TransportationScheduler : IScheduleTransport
  {
    private readonly Plant _plant;

    //TODO - Improve TransportationScheduler to include other algorithms

    private IWork _next_cargo;
    private IAcceptWorkorders _destination;

    public TransportationScheduler(Plant plant)
    {
      _plant = plant;
      _next_cargo = null;
      _destination = null;
      TransportTime = 0;
    }

    public IWork Cargo { get => _next_cargo; }
    public IAcceptWorkorders Destination { get => _destination; }
    public int TransportTime { get; private set; }

    public void ChooseNextCargo(IAcceptWorkorders current_location)
    {
      if(current_location.OutputBuffer.Count == 0)
      {
        _next_cargo = null;
        _destination = _plant.Workcenters.FirstOrDefault(x => x.OutputBuffer.Count > 0);
        if (_destination != null)
        {
          TransportTime = 5;
        }
        else
        {
          TransportTime = 0;
        }
        return;
      }

      _next_cargo = current_location.OutputBuffer.Peek();
      _destination = ChooseWorkcenter(_next_cargo.CurrentOpType);
      if(_destination == current_location)
      {
        TransportTime = 0;
      }
      else
      {
        TransportTime = 5;
      }
    }

    public IWork GetCargo(IAcceptWorkorders current_location)
    {
      if(_next_cargo == null){ return null; }
      return current_location.OutputBuffer.Dequeue();
    }

    private IAcceptWorkorders ChooseWorkcenter(string type)
    {
      return _plant.Workcenters.FirstOrDefault(x => x.ReceivesType(type));
    }
  }
}