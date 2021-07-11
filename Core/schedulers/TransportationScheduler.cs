namespace Core.Schedulers
{
  using System.Collections.Generic;
  using System.Linq;
  using Core.Workcenters;
  using Core.Plant;
  using Core.Resources;

  public interface IScheduleTransport
  {
    public IAcceptWorkorders Destination { get; }
    public IMes Mes { get; }
    public int TransportTime { get; }
    public void ScheduleNextStep(IAcceptWorkorders current_location, DayTime dt);
    public int? GetCargoID();
  }

  public class TransportationScheduler : IScheduleTransport
  {
    public enum Schedule { DEFAULT=0, BASIC=1 };
    private readonly IPlant _plant;
    private readonly Schedule _schedule;

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
      _schedule = (Schedule) Configuration.Instance.TransportationSchedule;
    }

    public void ScheduleNextStep(IAcceptWorkorders current_location, DayTime dt)
    {
      CargoID = null;

      if (_schedule == TransportationScheduler.Schedule.DEFAULT)
      {
        if(current_location.OutputBuffer.Any())
        {
          CargoID = ChooseCargoByAlgorithm(current_location);
        }
        Destination = ChooseWorkcenterByAlgorithm(current_location);
      }
      else
      {
        int? nextID = ChooseNextCargoByBasic(current_location);
        if (!nextID.HasValue) { 
          Destination = current_location; 
        }
        else if(current_location.OutputBuffer.Contains(nextID.Value))
        {
          CargoID = nextID;
          Destination = ChooseWorkcenterByBasic(current_location, dt);
        }
        else
        {
          Destination = _plant.Workcenters.Where(x => x.OutputBuffer.Contains(nextID.Value)).First();
        }
      }

      TransportTime = ShouldLeave(current_location) ? 0 : 3;
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

    private int? ChooseNextCargoByBasic(IAcceptWorkorders current_location)
    {
      var ratings = _plant.PlantScheduler.GetWorkorderRatings();
      ratings = ratings.Where(x => _plant.Workcenters.Select(y => y.OutputBuffer).Any(y => y.Contains(x.Object))).ToList();
      if(!ratings.Any()) { return null; }

      ratings.Where(x => current_location.OutputBuffer.Contains(x.Object)).ToList().ForEach(x => x.Value += Configuration.TransportJobTransportStayVariable);

      ratings.Where(x => CanBeWorkedOnAtCurrentLocation(x.Object)).ToList().ForEach(x => x.Value += Configuration.TransportJobStayVariable);

      int maxValue = ratings.Max(x => x.Value);
      return ratings.First(x => x.Value == maxValue).Object;
    }

    private bool CanBeWorkedOnAtCurrentLocation(int woid)
    {
      var wo = Mes.Workorders.First(x => x.Key == woid).Value;

      if(wo.CurrentOpIndex+1 >= wo.Operations.Count){ return false; }

      var wcName = Mes.LocationInventories.First(x => x.Value.Contains(wo)).Key;
      var wc = Mes.Locations[wcName];
      return wc.ReceivesType(wo.Operations[wo.CurrentOpIndex+1].Type);
    }

    private IAcceptWorkorders ChooseWorkcenterByAlgorithm(IAcceptWorkorders current_location)
    {
      return _schedule switch
      {
        _ => ChooseWorkcenterByDefault(current_location)
      };
    }

    private IAcceptWorkorders ChooseWorkcenterByBasic(IAcceptWorkorders current_location, DayTime dt)
    {
      var wo = current_location.OutputBuffer.Find(CargoID.Value);
      
      List<IAcceptWorkorders> availableWcs = _plant.Workcenters
          .Where(x => x.ReceivesType(wo.CurrentOpType))
          .ToList();
      
      if(availableWcs.Count() == 1)
      {
        return availableWcs.First();
      }

      if(availableWcs.Count() == 0)
      {
        return _plant.Dock();
      }

      var additionalWcs = _plant.GetEnterprise().Plants.First(x => x.Name != _plant.Name).Workcenters.Where(x => x.ReceivesType(wo.CurrentOpType)).ToList();

      List<Rating<IAcceptWorkorders>> wcRatings = availableWcs.Select(x => new Rating<IAcceptWorkorders>(x, 0)).ToList();
      wcRatings.AddRange(additionalWcs.Select(x => new Rating<IAcceptWorkorders>(x, Configuration.TransportWCAtCurrentPlantVariable)));

      wcRatings.ForEach(x => x.Value += GetWorkcenterValue(x.Object) * Configuration.TransportWCWaitVariable);

      wcRatings.ForEach(x => x.Value += GetWorkcenterJobCount((Workcenter)x.Object) * Configuration.TransportWCJobCountVariable);

      wcRatings.ForEach(x => x.Value += (((Workcenter) x.Object).IsAboutToBreakdown(dt) ? 1 : 0) * Configuration.MachineDowntimeVariable);

      var min = wcRatings.Min(x => x.Value);
      var wc = wcRatings.First(x => x.Value == min).Object;
      if (!_plant.Workcenters.Contains(wc)) { return _plant.Dock(); }
      return wc;
    }

    private int GetWorkcenterValue(IAcceptWorkorders workcenter)
    {
      return ((Machine)((Workcenter)workcenter).Machine).InputBuffer.Select(x => x.CurrentOpEstTimeToComplete + x.CurrentOpSetupTime).Sum();
    }

    private int GetWorkcenterJobCount(Workcenter workcenter)
    {
      return ((Machine)workcenter.Machine).InputBuffer.Count();
    }

    private IAcceptWorkorders ChooseWorkcenterByDefault(IAcceptWorkorders current_location)
    {
      if(CargoID.HasValue) // Transporting something
      {
        IWork cargo = current_location.OutputBuffer.Find(CargoID.Value);
        IAcceptWorkorders selected = _plant.Workcenters.FirstOrDefault(x => IsAppropriateWorkcenter(x, cargo));

        if( selected == null ) { return _plant.Dock(); }

        return selected;
      }
      else // Go to next destination
      {
        IAcceptWorkorders selected = _plant.Workcenters.FirstOrDefault(x => x.OutputBuffer.Any());
        if( selected == null ) { return current_location; }
        
        return selected;
      }
    }

    private bool IsAppropriateWorkcenter(IAcceptWorkorders subject, IWork wo)
    {
      return wo == null ? subject.OutputBuffer.Any() : subject.ReceivesType(wo.CurrentOpType);
    }

  }
}