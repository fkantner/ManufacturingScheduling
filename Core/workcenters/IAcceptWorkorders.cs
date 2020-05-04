namespace Core.Workcenters
{
  using Core.Plant;
  using Core.Resources;
  using System.Collections.Generic;

  public interface IAcceptWorkorders
  {
    string Name { get; }
    IEnumerable<IWork> OutputBuffer { get; }

    void AddToQueue(IWork wo);
    string ListOfValidTypes();
    bool ReceivesType(string type);
    void Work(DayTime dayTime);
    void SetMes(IMes mes);
  }
}