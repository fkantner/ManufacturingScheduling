namespace Core.Workcenters
{
  using Resources;
  using System.Collections.Generic;

  public class Workcenter : IAcceptWorkorders
  {
    private IDoWork _machine;
    private Quality _quality;
    private string _name;
    private readonly Queue<IWork> _outbound_buffer;
        
    public Workcenter(string name, IDoWork machine)
    {
      _machine = machine;
      _name = name;
      _outbound_buffer = new Queue<IWork>();
      _quality = new Quality();
    }

    public IDoWork Machine { get => _machine; }
    public string Name { get => _name; }
    public Queue<IWork> OutputBuffer { get => _outbound_buffer; }
    public Quality Inspection { get => _quality; }

    public void AddToQueue(IWork wo)
    {
      _machine.AddToQueue(wo);
      return;
    }

    public bool ReceivesType(string type)
    {
      return _machine.ReceivesType(type);
    }
    
    public void Work(DayTime dayTime)
    {
      // TODO - Implement QA part of Work Center
      IWork wo = Inspection.Work(dayTime);
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