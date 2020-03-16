namespace Core.Workcenters
{
  using Core.Resources;
  using System.Collections.Generic;

  public interface IAcceptWorkorders
  {
    public void AddToQueue(IWork wc);
    public bool ReceivesType(string type);
    public void Work(DayTime dayTime);

    public string Name { get; }
    public Queue<IWork> OutputBuffer { get; }
  }
}