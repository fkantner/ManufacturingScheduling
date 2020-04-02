namespace Core.Resources
{
  using System.Collections.Generic;

  public interface IWork
  {
    Op CurrentOp { get; }
    int CurrentOpEstTimeToComplete { get; }
    int CurrentOpSetupTime { get; }
    string CurrentOpType { get; }
    int Id { get; }
    void SetNextOp();
  }

  public class Workorder : IWork
  {
    private readonly List<Op> _operations;
    private int _currentOpIndex;

    public Workorder(int number, List<Op> ops)
    {
      _operations = ops;
      _currentOpIndex = 0;
      Id = number;
    }

    // PROPERTIES //
    // TODO - Implement a Count of Total ops and completed ops.
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

    public int Id { get; }

    // PUBLIC METHODS //
    public void SetNextOp()
    {
      if(_currentOpIndex < _operations.Count - 1)
        _currentOpIndex++;
      return;
    }
  }
}