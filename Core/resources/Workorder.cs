namespace Core.Resources
{
  using System.Collections.Generic;

  public class Workorder : IWork
  {
    public Workorder(int number, List<Op> ops, string pType)
    {
      Operations = ops;
      CurrentOpIndex = 0;
      Id = number;
      ProductType = pType;
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

    public string ProductType { get; }

    // PUBLIC METHODS //
    public void SetNextOp()
    {
      if(CurrentOpIndex < Operations.Count - 1)
        CurrentOpIndex++;
      return;
    }
  }
}