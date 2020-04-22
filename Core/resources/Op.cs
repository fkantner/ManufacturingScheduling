namespace Core.Resources
{
  using System;

  public class Op : ICloneable
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

    public object Clone()
    {
      return new Op(Type, EstTimeToComplete, SetupTime);
    }
  }
}