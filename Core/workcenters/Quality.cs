namespace Core.Workcenters
{
  using Core.Resources;
  using System.Collections.Generic;

  public class Quality
  {
    private Workorder _currentWo;
    private int _currentInspectionTime;
    private readonly int _inspectionTime;
    private Queue<Workorder> _buffer;

    public Quality()
    {
      _inspectionTime = 3;
      _currentInspectionTime = 0;
      _currentWo = null;
      _buffer = new Queue<Workorder>();
    }

    public Workorder CurrentWo { get => _currentWo; }
    public int CurrentInspectionTime { get => _currentInspectionTime; }
    public Queue<Workorder> Buffer { get => _buffer; }
    
    public void AddToQueue(Workorder workorder)
    {
      _buffer.Enqueue(workorder);
    }
    public Workorder Work(DayTime dayTime)
    {
      if(_currentWo == null)
      {
        if (_buffer.Count > 0) 
        {
          _currentWo = _buffer.Dequeue();
          _currentInspectionTime = _inspectionTime;
        }
        return null;
      }

      Workorder answer = null;
      _currentInspectionTime -= 1;
      if (_currentInspectionTime == 0)
      {
        answer = _currentWo;
        _currentWo = null;

        // TODO - Implement Psudo Random Success Inspections
        answer.SetNextOp();
      }

      return answer;
    }

  }
}