namespace Core.Resources.Virtual
{
  using Core.Resources;
  using System.Collections.Generic;
  using System;

  public class VirtualWorkorder : IWork
  {
    public enum Statuses { Open, InProgress, OnRoute, Complete }
    private Statuses _status;

    public VirtualWorkorder(int currentOp, IWork woToClone)
    {
      Id = woToClone.Id;
      Operations = new List<Op>();
      woToClone.Operations.ForEach((operation) => Operations.Add((Op) operation.Clone()));
      CurrentOpIndex = currentOp;
      _status = Statuses.Open;
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

    public void ChangeStatus(Statuses newStatus)
    {
      _status = newStatus;
    }
    public string GetStatus()
    {
      switch(_status)
      {
        case Statuses.Complete:
          return "Complete";
        case Statuses.InProgress:
          return "In Progress";
        case Statuses.OnRoute:
          return "On Route";
        default:
          return "Open";
      }
    }
    public void SetNextOp(){ CurrentOpIndex++; }
  }
}