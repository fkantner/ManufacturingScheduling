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
    public int? GetCargoID();
    public IAcceptWorkorders Destination { get; }
    public IMes Mes { get; }
    public int TransportTime { get; }
  }

  public class TransportationScheduler : IScheduleTransport
  {
    public enum Schedule { DEFAULT=0 };

    private readonly Plant _plant;
    private readonly Schedule _schedule;

    public TransportationScheduler(Plant plant, Schedule schedule=(Schedule) 0)
    {
      _plant = plant;
      CargoID = null;
      Destination = null;
      TransportTime = 0;
      _schedule = schedule;
    }

    public int? CargoID { get; private set; }
    public IAcceptWorkorders Destination { get; private set; }
    public IMes Mes { get => _plant.Mes; }
    public int TransportTime { get; private set; }

    public void ChooseNextCargo(IAcceptWorkorders current_location)
    {
      if(current_location.OutputBuffer.Any())
      {
        CargoID = ChooseCargoByAlgorithm(current_location);
      }
      else
      {
        CargoID = null;
      }
      SetDestination(current_location);
    }

    public int? GetCargoID()
    {
      return CargoID;
    }

    private void SetDestination(IAcceptWorkorders current_location)
    {
      bool staysHere = (!CargoID.HasValue && Destination == null) || (CargoID.HasValue && Destination == current_location);
      ChooseWorkcenterByAlgorithm(current_location);
      TransportTime = staysHere ? 0 : 5;
    }

    /*********************************/
    // Scheduling Algorithms go here //
    /*********************************/

    private int? ChooseCargoByAlgorithm(IAcceptWorkorders current_location)
    {
      // if _schedule == Schedule.DEFAULT
      return current_location.OutputBuffer.FirstId();
    }

    private void ChooseWorkcenterByAlgorithm(IAcceptWorkorders current_location)
    {
      // if _schedule == Schedule.DEFAULT
      if (!CargoID.HasValue)
      {
        Destination = _plant.Workcenters.FirstOrDefault(x => x.OutputBuffer.Any());
      }
      else
      {
        var cargo = current_location.OutputBuffer.Find(CargoID.Value);
        Destination = _plant.Workcenters.FirstOrDefault(x => x.ReceivesType(cargo.CurrentOpType));
      }
    }
  }
}