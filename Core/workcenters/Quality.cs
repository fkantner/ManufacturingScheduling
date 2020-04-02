namespace Core.Workcenters
{
  using Core.Resources;
  using System.Collections.Generic;

  public class Quality
  {
    private readonly int _inspectionTime;

    public Quality()
    {
      _inspectionTime = 3;
      CurrentInspectionTime = 0;
      CurrentWo = null;
      Buffer = new Queue<IWork>();
    }

    public IWork CurrentWo { get; private set; }
    public int CurrentInspectionTime { get; private set; }
    public Queue<IWork> Buffer { get; }

    public void AddToQueue(IWork workorder)
    {
      Buffer.Enqueue(workorder);
    }
    public IWork Work(DayTime dayTime)
    {
      if(CurrentWo == null)
      {
        if (Buffer.Count > 0)
        {
          CurrentWo = Buffer.Dequeue();
          CurrentInspectionTime = _inspectionTime;
        }
        return null;
      }

      IWork answer = null;
      CurrentInspectionTime--;
      if (CurrentInspectionTime == 0)
      {
        answer = CurrentWo;
        CurrentWo = null;

        // TODO - Implement Psudo Random Success Inspections
        answer.SetNextOp();
      }

      return answer;
    }
  }
}