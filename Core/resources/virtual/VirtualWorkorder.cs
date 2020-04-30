namespace Core.Resources.Virtual
{
  using Core.Resources;
  using System.Collections.Generic;

   public class VirtualWorkorder : IWork
    {
      public enum Statuses { Open, InProgress, OnRoute, Complete }

      public VirtualWorkorder(int currentOp, IWork woToClone)
      {
        Id = woToClone.Id;
        Operations = new List<Op>();
        woToClone.Operations.ForEach((operation) => Operations.Add((Op) operation.Clone()));
        CurrentOpIndex = currentOp;
      }

      public Op CurrentOp
      {
        get => Operations[CurrentOpIndex];
      }
      public int CurrentOpEstTimeToComplete { get => CurrentOp.EstTimeToComplete; }
      public int CurrentOpIndex { get; private set; }
      public int CurrentOpSetupTime { get => CurrentOp.SetupTime; }
      public string CurrentOpType { get => CurrentOp.Type; }
      public int Id { get; }
      public List<Op> Operations { get; }
      public string Status { get; private set; }

      public void SetNextOp(){ CurrentOpIndex++; }
    }
}