namespace Core.Workcenters
{
  using Resources;
  using System.Collections.Generic;

  public class Workcenter
  {
    private Machine _machine;
    private Quality _quality;
    private string _name;
    private readonly Queue<Workorder> _outbound_buffer;
        
    public Workcenter(string name, Machine machine)
    {
      _machine = machine;
      _name = name;
      _outbound_buffer = new Queue<Workorder>();
      _quality = new Quality();
    }

    public Machine Machine { get => _machine; }
    public string Name { get => _name; }
    public Queue<Workorder> OutputBuffer { get => _outbound_buffer; }
    public Quality Inspection { get => _quality; }

    public void AddToQueue(Workorder wc)
    {
      _machine.AddToQueue(wc);
      return;
    }

    public void Work(DayTime dayTime)
    {
      // TODO - Implement QA part of Work Center
      Workorder wo = Inspection.Work(dayTime);
      if(wo != null)
      {
        // TODO - Implement Rework and Scheduling
        OutputBuffer.Enqueue(wo);
        // TODO - Implement Notify Scheduler when WC done
      }

      wo = _machine.Work(dayTime);

      if(wo != null)
      {
        // TODO - Work Center Report to MES when finishing Work
        //_outbound_buffer.Enqueue(wo);
        Inspection.AddToQueue(wo);
      }
    }

  }
}