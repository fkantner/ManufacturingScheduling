namespace Core.Workcenters
{
  using System.Collections.Generic;
  using Core.Plant;
  using Core.Resources;

  public interface IDoWork
  {
    void AddPlant(Plant plant);
    void AddToQueue(IWork workorder);
    bool ReceivesType(Op.OpTypes type);
    IWork Work(DayTime dayTime);
    List<Op.OpTypes> ListOfValidTypes();
    IWork CurrentWorkorder { get; }
  }
}