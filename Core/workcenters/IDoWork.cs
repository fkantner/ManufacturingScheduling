namespace Core.Workcenters
{
  using Core.Resources;

  public interface IDoWork
  {
    public void AddToQueue(Workorder workorder);
    public bool ReceivesType(string type);
    public Workorder Work(DayTime dayTime);
  }
}