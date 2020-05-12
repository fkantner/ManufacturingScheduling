namespace Core.Workcenters
{
  using Core.Plant;
  using Core.Resources;

  public interface IAcceptWorkorders
  {
    string Name { get; }
    ICustomQueue OutputBuffer { get; }

    void AddToQueue(IWork wo);
    string ListOfValidTypes();
    bool ReceivesType(string type);
    void Work(DayTime dayTime);
    void SetMes(IMes mes);
  }
}