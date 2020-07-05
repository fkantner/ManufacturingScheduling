namespace Core.Schedulers
{
  using System.Linq;
  using Core.Workcenters;
  using Core.Plant;
  using Core.Resources;

  public interface IScheduleTransport
  {
    public IAcceptWorkorders Destination { get; }
    public IMes Mes { get; }
    public int TransportTime { get; }
    public void ScheduleNextStep(IAcceptWorkorders current_location);
    public int? GetCargoID();
  }

  public class TransportationScheduler : IScheduleTransport
  {
    public enum Schedule { DEFAULT=0 };

    public int? CargoID { get; private set; }
    public IAcceptWorkorders Destination { get; private set; }
    public IMes Mes { get => _plant.Mes; }
    public int TransportTime { get; private set; }

    public TransportationScheduler(IPlant plant)
    {
      _plant = plant;
      CargoID = null;
      Destination = null;
      TransportTime = 0;
      _schedule = (Schedule) Configuration.TransportationSchedule;
    }

    public void ScheduleNextStep(IAcceptWorkorders current_location)
    {
      CargoID = null;
      if(current_location.OutputBuffer.Any())
      {
        CargoID = ChooseCargoByAlgorithm(current_location);
      }

      Destination = ChooseWorkcenterByAlgorithm(current_location);
      TransportTime = ShouldLeave(current_location) ? 0 : 5;
    }

    public int? GetCargoID()
    {
      return CargoID;
    }

    /*********************************/
    // Scheduling Algorithms go here //
    /*********************************/

    private bool ShouldLeave(IAcceptWorkorders current_location)
    {
      return (!CargoID.HasValue && Destination == null) || (CargoID.HasValue && Destination == current_location);
    }

    private int? ChooseCargoByAlgorithm(IAcceptWorkorders current_location)
    {
      return _schedule switch
      {
        _ => ChooseCargoByDefault(current_location)
      };
    }

    private int? ChooseCargoByDefault(IAcceptWorkorders current_location)
    {
      int? selected = current_location.OutputBuffer.FirstId();
      return _plant.PlantScheduler.ValidateWoForTransport(selected, current_location.Name);
    }

    private IAcceptWorkorders ChooseWorkcenterByAlgorithm(IAcceptWorkorders current_location)
    {
      return _schedule switch
      {
        _ => ChooseWorkcenterByDefault(current_location)
      };
    }

    private IAcceptWorkorders ChooseWorkcenterByDefault(IAcceptWorkorders current_location)
    {
      
      IWork cargo = CargoID.HasValue ? current_location.OutputBuffer.Find(CargoID.Value) : null;
      IAcceptWorkorders selected = _plant.Workcenters.FirstOrDefault(x => IsAppropriateWorkcenter(x, cargo));

      string selectedName = selected?.Name;
      string new_selected = _plant.PlantScheduler.ValidateDestinationForTransport(CargoID, current_location.Name, selectedName);
      
      if(new_selected == selectedName) { return selected; }
      if(new_selected == null) { return null; }

      return _plant.Workcenters.FirstOrDefault(x => x.Name == new_selected);
    }

    private bool IsAppropriateWorkcenter(IAcceptWorkorders subject, IWork wo)
    {
      return wo == null ? subject.OutputBuffer.Any() : subject.ReceivesType(wo.CurrentOpType);
    }

    private readonly IPlant _plant;
    private readonly Schedule _schedule;
  }
}