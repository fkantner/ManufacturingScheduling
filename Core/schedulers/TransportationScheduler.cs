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
    public IMes Mes { get; }
    public int TransportTime { get; }
  }

  public class TransportationScheduler : IScheduleTransport
  {
    private readonly Plant _plant;
    private readonly Schedule _schedule;

    //TODO - Improve TransportationScheduler to include other algorithms

    public enum Schedule { DEFAULT=0 };

    public TransportationScheduler(Plant plant, Schedule schedule=(Schedule) 0)
    {
      _plant = plant;
      Cargo = null;
      Destination = null;
      TransportTime = 0;
      _schedule = schedule;
    }

    public IWork Cargo { get; private set; }
    public IAcceptWorkorders Destination { get; private set; }
    public IMes Mes { get => _plant.Mes; }
    public int TransportTime { get; private set; }

    public void ChooseNextCargo(IAcceptWorkorders current_location)
    {
      if(current_location.OutputBuffer.Any())
      {
        Cargo = ChooseCargoByAlgorithm(current_location);
      }
      else
      {
        Cargo = null;
      }
      SetDestination(current_location);
    }

    public IWork GetCargo(IAcceptWorkorders current_location)
    {
      if(Cargo == null){ return null; }
      return (current_location.OutputBuffer as Queue<IWork>)?.Dequeue();
    }

    private void SetDestination(IAcceptWorkorders current_location)
    {
      bool staysHere = (Cargo == null && Destination == null) || (Cargo != null && Destination == current_location);
      ChooseWorkcenterByAlgorithm();
      TransportTime = staysHere ? 0 : 5;
    }

    // Scheduling Algorithms go here //
    private IWork ChooseCargoByAlgorithm(IAcceptWorkorders current_location)
    {
      // if _schedule == Schedule.DEFAULT
      return (current_location.OutputBuffer as Queue<IWork>)?.Peek();
    }

    private void ChooseWorkcenterByAlgorithm()
    {
      // if _schedule == Schedule.DEFAULT
      if (Cargo == null)
      {
        Destination = _plant.Workcenters.FirstOrDefault(x => x.OutputBuffer.Any());
      }
      else
      {
        Destination = _plant.Workcenters.FirstOrDefault(x => x.ReceivesType(Cargo.CurrentOpType));
      }
    }

  }
}