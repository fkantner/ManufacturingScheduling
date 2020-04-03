namespace Core.Resources
{
   public interface IWork
  {
    Op CurrentOp { get; }
    int CurrentOpEstTimeToComplete { get; }
    int CurrentOpSetupTime { get; }
    string CurrentOpType { get; }
    int Id { get; }
    void SetNextOp();
  }
}