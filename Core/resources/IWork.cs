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
    Op.OpTypes CurrentOpType { get; }
    int Id { get; }
    Workorder.PoType ProductType { get; }
    void SetNextOp();
  }
}