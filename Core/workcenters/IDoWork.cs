namespace Core.Workcenters
{
  using Core.Resources;

  public interface IDoWork
  {
    public void AddToQueue(IWork workorder);
    public bool ReceivesType(string type);
    public IWork Work(DayTime dayTime);
  }
}