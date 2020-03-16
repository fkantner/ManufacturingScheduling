namespace Core.Workcenters
{
  using Core.Resources;
  using System.Collections.Generic;

  public class Quality
  {
    private IWork _currentWo;
    private int _currentInspectionTime;
    private readonly int _inspectionTime;
    private Queue<IWork> _buffer;

    public Quality()
    {
      _inspectionTime = 3;
      _currentInspectionTime = 0;
      _currentWo = null;
      _buffer = new Queue<IWork>();
    }

    public IWork CurrentWo { get => _currentWo; }
    public int CurrentInspectionTime { get => _currentInspectionTime; }
    public Queue<IWork> Buffer { get => _buffer; }
    
    public void AddToQueue(IWork workorder)
    {
      _buffer.Enqueue(workorder);
    }
    public IWork Work(DayTime dayTime)
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

      IWork answer = null;
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