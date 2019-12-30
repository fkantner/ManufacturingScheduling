namespace Core.Workcenters
{
  using Core.Resources;
  using Core.Schedulers;
  using System.Collections.Generic;

  public class Machine
  {
    private Workorder _currentWorkorder;
    private int _estTimeToComplete;
    private int _setupTime;
    private string _lastType;
    private readonly Queue<Workorder> _queue;
    private readonly MachineScheduler _scheduler;
    private readonly List<string> _type;
    private readonly string _name;

    public Machine(string name, MachineScheduler ms, List<string> type)
    {
      _name = name;
      _queue = new Queue<Workorder>();
      _scheduler = ms;
      _type = type;
      _setupTime = 0;
      _estTimeToComplete = 0;
      _lastType = null;
      _currentWorkorder = null;
    }

    public Workorder CurrentWorkorder
    {
      get => _currentWorkorder;
      private set => _currentWorkorder = value;
    }
    public int EstTimeToComplete 
    { 
      get => _estTimeToComplete; 
      private set => _estTimeToComplete = value;
    }
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
    
    public void AddToQueue(Workorder wc)
    {
      _queue.Enqueue(wc);
      _scheduler.Sort(_queue);
      return;
    }

    public override string ToString()
    {
      string answer = "Name: " + Name + " Types: {";
      foreach (string type in _type)
      {
        answer += " " + type + " ";
      }
      answer += "}";

      if( CurrentWorkorder != null)
      {
        answer += "\n\tCurrent WO: " + CurrentWorkorder.Id;
      }

      if(_queue.Count > 0)
      {
        answer += "\n\tQueue:\n";
        foreach(Workorder wc in _queue)
        {
          answer += "\t" + wc.ToString() + "\n";
        }
      }

      return answer;
    } // ToString()

    public Workorder Work(DayTime dayTime)
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

      if (EstTimeToComplete > 1) { return null; }

      Workorder answer = CurrentWorkorder;
      LastType = CurrentWorkorder.CurrentOpType;
      CurrentWorkorder = null;
      return answer;
    } // Work()

  }
}
