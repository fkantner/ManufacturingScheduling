namespace Core.Workcenters
{
  using Core.Resources;
  using System.Collections.Generic;

  public interface IAcceptWorkorders
  {
    public void AddToQueue(Workorder wc);
    public bool ReceivesType(string type);
    public void Work(DayTime dayTime);

    public Queue<Workorder> OutputBuffer { get; }
  }
}