namespace Core.Resources
{
  using System.Collections.Generic;

  public class Workorder : IWork
  {
    public Workorder(int number, List<Op> ops)
    {
      Operations = ops;
      CurrentOpIndex = 0;
      Id = number;
    }

    // PROPERTIES //
    public int CountCompletedOps
    {
      get => CurrentOpIndex;
    }

    public int CountTotalOps
    {
      get => Operations.Count;
    }

    public Op CurrentOp
    {
      get => Operations[CurrentOpIndex];
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

    public int CurrentOpIndex { get; private set;}

    public List<Op> Operations { get; }

    // PUBLIC METHODS //
    
    public void SetNextOp()
    {
      if(CurrentOpIndex < Operations.Count - 1)
        CurrentOpIndex++;
      return;
    }
  }
}