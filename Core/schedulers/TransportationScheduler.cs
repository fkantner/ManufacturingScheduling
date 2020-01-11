namespace Core.Schedulers
{
  using Core.Resources;
  using Core.Workcenters;
  using Core.Plant;
  using System.Linq;

  public class TransportationScheduler
  {
    private Plant _plant;

    //TODO - Improve TransportationScheduler to include other algorithms

    private Workorder _next_cargo;
    private IAcceptWorkorders _destination;
    private int _transport_time;
    
    public TransportationScheduler(Plant plant)
    {
      _plant = plant;
      _next_cargo = null;
      _destination = null;
      _transport_time = 0;
    }

    public Workorder Cargo { get => _next_cargo; }
    public IAcceptWorkorders Destination { get => _destination; }
    public int TransportTime { get => _transport_time; }

    public void ChooseNextCargo(IAcceptWorkorders current_location)
    {
      if(current_location.OutputBuffer.Count == 0)
      {
        _next_cargo = null;
        _destination = _plant.Workcenters.FirstOrDefault(x => x.OutputBuffer.Count > 0);
        if (_destination != null)
        {
          _transport_time = 5;
        }
        else
        {
          _transport_time = 0;
        }
        return;
      }

      _next_cargo = current_location.OutputBuffer.Peek();
      _destination = ChooseWorkcenter(_next_cargo.CurrentOpType);
      if(_destination == current_location)
      {
        _transport_time = 0;
      }
      else
      {
        _transport_time = 5;
      }
    }

    public Workorder GetCargo(IAcceptWorkorders current_location)
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