namespace Core.Resources
{
  public class Op
  {
    private readonly string _type;
    private readonly int _estTimeToComplete;
    private readonly int _setupTime;

    public Op(string type, int completeTime, int setupTime)
    {
      _type = type;
      _estTimeToComplete = completeTime;
      _setupTime = setupTime;
    }

    public string Type { get => _type; }
    public int EstTimeToComplete { get => _estTimeToComplete; }
    public int SetupTime { get => _setupTime; }

  }
}