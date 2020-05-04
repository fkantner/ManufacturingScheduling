namespace Core.Workcenters
{
  using Core.Plant;
  using Core.Resources;
  using System.Collections.Generic;

  public interface IAcceptWorkorders
  {
    public void AddToQueue(IWork wo);
    public bool ReceivesType(string type);
    public void SetMes(IMes mes);
    public void Work(DayTime dayTime);

    public string ListOfValidTypes();

    public string Name { get; }
    public IEnumerable<IWork> OutputBuffer { get; }
  }
}