namespace Core.Resources
{
  public class Op
  {
    private readonly string TYPE;
    private readonly int EST_TIME_TO_COMPLETE;
    private readonly int SETUP_TIME;

    public Op(string type, int completeTime, int setupTime)
    {
      TYPE = type;
      EST_TIME_TO_COMPLETE = completeTime;
      SETUP_TIME = setupTime;
    }

    public string Type() { return TYPE; }
    public int EstTimeToComplete() { return EST_TIME_TO_COMPLETE; }
    public int SetupTime() { return SETUP_TIME; }

    public override string ToString()
    {
      return "TYPE: " + Type() + " SetupTime: " + SetupTime() + " EstimatedTimeToComplete: " + EstTimeToComplete();
    }
  }
}