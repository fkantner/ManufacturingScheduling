namespace Core.Schedulers
{
  using Core.Resources;
  using Core.Workcenters;
  using Core.Plant;
  using System.Linq;
  using System.Collections.Generic;

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

    public TransportationScheduler(Plant plant)
    {
      _plant = plant;
      Cargo = null;
      Destination = null;
      TransportTime = 0;
    }

    public IWork Cargo { get; private set; }
    public IAcceptWorkorders Destination { get; private set; }
    public int TransportTime { get; private set; }

    public void ChooseNextCargo(IAcceptWorkorders current_location)
    {
      if(current_location.OutputBuffer.Count == 0)
      {
        Cargo = null;
        Destination = _plant.Workcenters.FirstOrDefault(x => x.OutputBuffer.Count > 0);
        if (Destination != null)
        {
          TransportTime = 5;
        }
        else
        {
          TransportTime = 0;
        }
        return;
      }

      Cargo = (current_location.OutputBuffer as Queue<IWork>)?.Peek();
      Destination = ChooseWorkcenter(Cargo.CurrentOpType);
      if(Destination == current_location)
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
      if(Cargo == null){ return null; }
      return (current_location.OutputBuffer as Queue<IWork>)?.Dequeue();
    }

    private IAcceptWorkorders ChooseWorkcenter(string type)
    {
      return _plant.Workcenters.FirstOrDefault(x => x.ReceivesType(type));
    }
  }
}