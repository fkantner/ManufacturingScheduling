namespace Core.Workcenters
{
  using Core.Plant;
  using Core.Resources;

  public interface IDoWork
  {
    void AddPlant(Plant plant);
    void AddToQueue(IWork workorder);
    bool ReceivesType(string type);
    IWork Work(DayTime dayTime);
    string ListOfValidTypes();
    IWork CurrentWorkorder { get; }
  }
}