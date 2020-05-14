namespace Core.Workcenters
{
  using Core.Plant;
  using Core.Resources;
  using Core.Schedulers;
  using System.Collections.Generic;

  public class Machine : IDoWork
  {
    private readonly IScheduleMachines _scheduler;
    private readonly List<string> _type;

    public Machine(string name, IScheduleMachines ms, List<string> type)
    {
      Name = "Machine " + name;
      _scheduler = ms;
      _type = type;

      CurrentWorkorder = null;
      EstTimeToComplete = 0;
      InputBuffer = new NeoQueue();
      LastType = null;
      SetupTime = 0;
    }

    public IWork CurrentWorkorder { get; private set; }
    public int EstTimeToComplete { get; private set; }
    public ICustomQueue InputBuffer { get; }
    public string LastType { get; private set; }
    public string Name { get; }
    public int SetupTime { get; private set; }

    public void AddPlant(Plant plant)
    {
      _scheduler.AddPlant(plant);
    }

    public void AddToQueue(IWork wc)
    {
      InputBuffer.Enqueue(wc);
      return;
    }

    public bool ReceivesType(string type)
    {
      return _type.Contains(type);
    }

    public string ListOfValidTypes()
    {
      return string.Join(",", _type);
    }

    public IWork Work(DayTime dayTime)
    {
      // TODO - Implement Machine Breakdown

      // TODO - Implement Machine Tooling

      // TODO - Implement Machine Resourcing

      if (CurrentWorkorder == null)
      {
        if(InputBuffer.Empty()){ return null; }

        int nextWoId = _scheduler.ChooseNextWoId(Name, InputBuffer);
        CurrentWorkorder = InputBuffer.Remove(nextWoId);
        if ( LastType == null || LastType != CurrentWorkorder.CurrentOpType )
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

      // TODO - Implement Machine Tooling replacement

      // TODO - Implement Machine Resource replacement

      if (EstTimeToComplete > 0) { return null; }

      IWork answer = CurrentWorkorder;
      LastType = CurrentWorkorder.CurrentOpType;
      CurrentWorkorder = null;
      return answer;
    } // Work()
  }
}
