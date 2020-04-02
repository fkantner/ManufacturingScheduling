namespace Core.Workcenters
{
  using Core.Resources;
  using Core.Schedulers;
  using System.Collections.Generic;

  public class Machine : IDoWork
  {
    private readonly IScheduleMachines _scheduler;
    private readonly List<string> _type;

    public Machine(string name, IScheduleMachines ms, List<string> type)
    {
      Name = name;
      _scheduler = ms;
      _type = type;

      CurrentWorkorder = null;
      EstTimeToComplete = 0;
      InputBuffer = new Queue<IWork>();
      LastType = null;
      SetupTime = 0;
    }

    public IWork CurrentWorkorder { get; private set; }
    public int EstTimeToComplete { get; private set; }
    public Queue<IWork> InputBuffer { get; }
    public string LastType { get; private set; }
    public string Name { get; }
    public int SetupTime { get; private set; }

    public void AddToQueue(IWork wc)
    {
      InputBuffer.Enqueue(wc);
      _scheduler.Sort(InputBuffer);
      return;
    }

    public bool ReceivesType(string type)
    {
      return _type.Contains(type);
    }

    public IWork Work(DayTime dayTime)
    {
      // TODO - Implement Machine Breakdown

      // TODO - Implement Machine Tooling

      // TODO - Implement Machine Resourcing

      if (CurrentWorkorder == null)
      {
        if(InputBuffer.Count == 0){ return null; }

        CurrentWorkorder = InputBuffer.Dequeue();
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
