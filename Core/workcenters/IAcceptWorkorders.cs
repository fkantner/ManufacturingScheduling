namespace Core.Workcenters
{
  using Core.Plant;
  using Core.Resources;
  using System.Collections.Generic;

  public interface IAcceptWorkorders
  {
    string Name { get; }
    ICustomQueue OutputBuffer { get; }

    void AddPlant(IPlant plant);
    void AddToQueue(IWork wo);
    List<Op.OpTypes> ListOfValidTypes();
    bool ReceivesType(Op.OpTypes type);
    void Work(DayTime dayTime);
    void SetMes(IMes mes);
  }
}