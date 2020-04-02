namespace Core.Resources
{
  public class Op
  {
    public Op(string type, int completeTime, int setupTime)
    {
      Type = type;
      EstTimeToComplete = completeTime;
      SetupTime = setupTime;
    }

    public string Type { get; }
    public int EstTimeToComplete { get; }
    public int SetupTime { get; }
  }
}