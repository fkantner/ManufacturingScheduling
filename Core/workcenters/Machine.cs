namespace Core.Workcenters
{
  using Core.Resources;
  using Core.Schedulers;
  using System.Collections.Generic;

  public class Machine : IDoWork
  {
    private IWork _currentWorkorder;
    private int _estTimeToComplete;
    private int _setupTime;
    private string _lastType;
    private readonly Queue<IWork> _queue;
    private readonly IScheduleMachines _scheduler;
    private readonly List<string> _type;
    private readonly string _name;

    public Machine(string name, IScheduleMachines ms, List<string> type)
    {
      _name = name;
      _queue = new Queue<IWork>();
      _scheduler = ms;
      _type = type;
      _setupTime = 0;
      _estTimeToComplete = 0;
      _lastType = null;
      _currentWorkorder = null;
    }

    public IWork CurrentWorkorder
    {
      get => _currentWorkorder;
      private set => _currentWorkorder = value;
    }
    public int EstTimeToComplete 
    { 
      get => _estTimeToComplete; 
      private set => _estTimeToComplete = value;
    }
    public Queue<IWork> InputBuffer { get => _queue; }
    public string LastType 
    { 
      get => _lastType; 
      private set => _lastType = value;
    }
    public string Name { get => _name; }
    public int SetupTime 
    { 
      get => _setupTime; 
      private set => _setupTime = value;
    }

    public void AddToQueue(IWork wc)
    {
      _queue.Enqueue(wc);
      _scheduler.Sort(_queue);
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
        if(_queue.Count == 0){ return null; }

        CurrentWorkorder = _queue.Dequeue();
        if ( LastType == null || LastType != CurrentWorkorder.CurrentOpType )
        {
          SetupTime = CurrentWorkorder.CurrentOpSetupTime;
        }

        EstTimeToComplete = CurrentWorkorder.CurrentOpEstTimeToComplete;
        return null;
      }

      if(SetupTime > 0)
      {
        SetupTime = SetupTime - 1;
        return null;
      }

      EstTimeToComplete = EstTimeToComplete - 1;

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
