namespace Core.Resources
{
  using System.Collections.Generic;

   public interface IWork
  {
    Op CurrentOp { get; }
    int CurrentOpIndex { get; }
    List<Op> Operations { get; }
    int CurrentOpEstTimeToComplete { get; }
    int CurrentOpSetupTime { get; }
    string CurrentOpType { get; }
    int Id { get; }
    void SetNextOp();
  }
}