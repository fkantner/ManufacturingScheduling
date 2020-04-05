namespace Core.Workcenters
{
  using Resources;
  using System.Collections.Generic;

  public class Workcenter : IAcceptWorkorders
  {
    public Workcenter(string name, IDoWork machine)
    {
      Machine = machine;
      Name = name;
      _output_buffer = new Queue<IWork>();
      Inspection = new Quality();
    }

    public IDoWork Machine { get; }
    public string Name { get; }
    private readonly Queue<IWork> _output_buffer;
    public IEnumerable<IWork> OutputBuffer
    {
      get { return _output_buffer; }
    }
    public Quality Inspection { get; }

    public void AddToQueue(IWork wo)
    {
      Machine.AddToQueue(wo);
      return;
    }

    public bool ReceivesType(string type)
    {
      return Machine.ReceivesType(type);
    }

    public void Work(DayTime dayTime)
    {
      // TODO - Implement QA part of Work Center
      IWork wo = Inspection.Work(dayTime);

      if(wo != null)
      {
        // TODO - Implement Rework and Scheduling
        _output_buffer.Enqueue(wo);
        // TODO - Implement Notify Scheduler when WC done
      }

      wo = Machine.Work(dayTime);

      if(wo != null)
      {
        // TODO - Work Center Report to MES when finishing Work
        Inspection.AddToQueue(wo);
      }
    }
  }
}