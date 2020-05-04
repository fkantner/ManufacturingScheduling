namespace Core.Workcenters
{
  using Core.Resources;

  public interface IDoWork
  {
    void AddToQueue(IWork workorder);
    bool ReceivesType(string type);
    IWork Work(DayTime dayTime);
    string ListOfValidTypes();
    IWork CurrentWorkorder { get; }
  }
}