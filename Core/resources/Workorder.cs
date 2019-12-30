namespace Core.Resources
{
  using System.Collections.Generic;

  public class Workorder
  {
    private readonly List<Op> _operations;
    private int _currentOpIndex;
    private readonly int _id;

    public Workorder(int number, List<Op> ops)
    {
      _operations = ops;
      _currentOpIndex = 0;
      _id = number;
    }

    // PROPERTIES //

    public Op CurrentOp
    {
      get => _operations[_currentOpIndex];
    }

    public int CurrentOpEstTimeToComplete
    {
      get => CurrentOp.EstTimeToComplete;
    }

    public int CurrentOpSetupTime
    {
      get => CurrentOp.SetupTime;
    }

    public string CurrentOpType
    {
      get => CurrentOp.Type;
    }

    public int Id { get => _id; }

    // PUBLIC METHODS //

    public void SetNextOp()
    {
      _currentOpIndex++;
      return;
    }

    public override string ToString()
    {
      string answer = "WorkOrder: " + Id + "\n";
      for(int i = 0; i<_operations.Count; i++)
      {
        string part = "\t" + _operations[i].ToString();
        if (i == _currentOpIndex)
        {
          part = part + " <= CURRENT";
        }
        part = part + "\n";
        answer = answer + part;
      }
      return answer;
    }
  }
}