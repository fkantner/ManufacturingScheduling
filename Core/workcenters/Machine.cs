namespace Core.Workcenters
{
  using System.Collections.Generic;
  using Core.Plant;
  using Core.Resources;
  using Core.Schedulers;

  public class Machine : IDoWork
  {
    public enum Types {
      SmallDrill,BigDrill,Lathe,WetCnc,DryCnc,Press
    }

    // Public Properties
    public IWork CurrentWorkorder { get; private set; }
    public int EstTimeToComplete { get; private set; }
    public ICustomQueue InputBuffer { get; }
    public Op.OpTypes LastType { get; private set; }
    public string Name { get; }
    public int SetupTime { get; private set; }

    // Private Members
    private readonly IScheduleMachines _scheduler;
    private readonly Types _type;
    private readonly List<Op.OpTypes> _validTypes;

    public Machine(string name, Types type)
    {
      Name = name;
      _scheduler = new MachineScheduler();
      _type = type;
      _validTypes = GetValidTypes(type);

      CurrentWorkorder = null;
      EstTimeToComplete = 0;
      InputBuffer = new NeoQueue();
      LastType = _validTypes[0];
      SetupTime = 0;
    }

    public void AddPlant(IPlant plant)
    {
      _scheduler.AddPlant(plant);
    }

    public void AddToQueue(IWork wc)
    {
      InputBuffer.Enqueue(wc);
    }

    public bool ReceivesType(Op.OpTypes type)
    {
      return _validTypes.Contains(type);
    }

    public List<Op.OpTypes> ListOfValidTypes()
    {
      return _validTypes;
    }

    public IWork Work(DayTime dayTime)
    {
      // TODO - Implement Machine Breakdown

      if (CurrentWorkorder == null)
      {
        if(InputBuffer.Empty()){ return null; }

        int nextWoId = _scheduler.ChooseNextWoId(Name, InputBuffer);
        CurrentWorkorder = InputBuffer.Remove(nextWoId);
        if ( LastType != CurrentWorkorder.CurrentOpType )
        {
          SetupTime = CurrentWorkorder.CurrentOpSetupTime;
        }

        EstTimeToComplete = CurrentWorkorder.CurrentOpEstTimeToComplete;
        return null;
      }

      if(SetupTime > 0)
      {
        SetupTime--;
        return null;
      }

      EstTimeToComplete--;

      if (EstTimeToComplete > 0) { return null; }

      IWork answer = CurrentWorkorder;
      LastType = CurrentWorkorder.CurrentOpType;
      CurrentWorkorder = null;
      return answer;
    } // Work()

    private List<Op.OpTypes> GetValidTypes(Types machineType)
    {
      return machineType switch {
        Types.SmallDrill => new List<Op.OpTypes>(){ 
          Op.OpTypes.DrillOpType2, 
          Op.OpTypes.DrillOpType3 
        },
        Types.BigDrill => new List<Op.OpTypes>(){ 
          Op.OpTypes.DrillOpType1, 
          Op.OpTypes.DrillOpType2
        },
        Types.Lathe => new List<Op.OpTypes>(){ 
          Op.OpTypes.LatheOpType1, 
          Op.OpTypes.LatheOpType2
        },
        Types.WetCnc => new List<Op.OpTypes>(){ 
          Op.OpTypes.CncOpType1,
          Op.OpTypes.CncOpType2
        },
        Types.DryCnc => new List<Op.OpTypes>(){ 
          Op.OpTypes.CncOpType2,
          Op.OpTypes.CncOpType3,
          Op.OpTypes.CncOpType4
        },
        Types.Press => new List<Op.OpTypes>(){ 
          Op.OpTypes.PressOpType1,
          Op.OpTypes.PressOpType2
        },
        _ => new List<Op.OpTypes>()
      };
    }
  }
}
